using mvc.Data;
using mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using NToastNotify;

using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

using static mvc.MVCConstants.POLICIES;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text;


namespace mvc.Controllers
{
    [Authorize(Policy = APP_POLICY.NAME)]
    public class AppointmentController : Controller
    {
        private readonly ILogger<AppointmentController> _logger;
        private readonly IToastNotification _toastNotification;

        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IHtmlLocalizer<Resource> _sharedLocalizer ;

        public AppointmentController(ILogger<AppointmentController> logger,
                                    IToastNotification toastNotification,
                                    IHtmlLocalizer<Resource> localizer,
                                    IEmailSender emailSender,
                                    ApplicationDbContext context)
        {
            _logger = logger;
            _toastNotification = toastNotification;
            _sharedLocalizer = localizer;
            _context = context;
            _emailSender = emailSender;
        }


        public IActionResult Index()
        {
            IEnumerable<Appointment> appointments = _context.Appointment
                                                            .Include(ap => ap.Client)
                                                            .Include(ap => ap.Staff)
                                                            .ToList();

            return View(appointments);
        }

        [HttpGet]
        public IActionResult Details(int ID)
        {
            Appointment? appointment = _context.Appointment
                                                            .Include(ap => ap.Client)
                                                            .Include(ap => ap.Staff)
                                                            .Where(ap => ap.ID == ID)
                                                            .Single();

            if (appointment is null)
            {
                _toastNotification.AddErrorToastMessage($"Error : the Appointment #{ID} does not exists.");

                return RedirectToAction(nameof(Index));
            }

            return View(appointment);
        }


        [HttpGet]        
        [Authorize(Policy = APP_POLICY_EDITABLE_CRUD.NAME)]
        public IActionResult Create()
        {
            ViewBag.ClientList = new SelectList(_context.Clients, "ID", "Name");
            ViewBag.StaffList = new SelectList(_context.Staff, "ID", "Name");

            return View();
        }

        [HttpPost]
        [Authorize(Policy = APP_POLICY_EDITABLE_CRUD.NAME)]
        public IActionResult Create(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<Appointment> repeatedAppointmentNumbers = _context.Appointment
                                            .Where(ap => ap.AppoitmentNumber == appointment.AppoitmentNumber)
                                            .ToList();

                if (repeatedAppointmentNumbers.Count() == 0)
                {
                    _context.Appointment.Add(appointment);
                    _context.SaveChanges();

                    _toastNotification.AddSuccessToastMessage($"Successfully schedule Appointment # {appointment.AppoitmentNumber}");

                    
                    this.SendEmail (appointment);

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = _sharedLocalizer["ErrorAppointementRepeated"];
                }
            }
            _toastNotification.AddErrorToastMessage($"Error while schedulling Appointment #{appointment.AppoitmentNumber}.");
            ViewBag.ClientList = new SelectList(_context.Clients, "ID", "Name");
            ViewBag.StaffList = new SelectList(_context.Staff, "ID", "Name");


            ViewBag.ErrorMessage = _sharedLocalizer["ErrorAppointementDataNotValid"];

            return View(appointment);
            
        }

        [HttpGet]
        [Authorize(Policy = APP_POLICY_EDITABLE_CRUD.NAME)]
        public IActionResult Edit(int ID)
        {
            Appointment? appointment = _context.Appointment.Find(ID);

             if (appointment is null)
            {
                _toastNotification.AddErrorToastMessage($"Error : the Appointment #{ID} does not exists.");

                return RedirectToAction(nameof(Index));
            }

            ViewBag.ClientList = new SelectList(_context.Clients, "ID", "Name", appointment.ClientID);
            ViewBag.StaffList = new SelectList(_context.Staff, "ID", "Name", appointment.StaffID );

            return View(appointment);
        }

        [HttpPost]
        [Authorize(Policy = APP_POLICY_EDITABLE_CRUD.NAME)]
        public IActionResult Edit(Appointment appointmentToUpdate)
        {


            try
            {

                _context.Appointment.Update(appointmentToUpdate);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {

                _toastNotification.AddErrorToastMessage($"Error saving Appointment #{appointmentToUpdate.AppoitmentNumber}.");

            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = APP_POLICY_ADMIN.NAME)]
        public IActionResult DeleteConfirm(int ID)
        {

            Appointment? appointment = _context.Appointment.Find(ID);

            if (appointment is null)
            {
                _toastNotification.AddErrorToastMessage($"Error while deleting Appointment #{ID}.");

                return RedirectToAction(nameof(Index));
            }
            _context.Appointment.Remove(appointment);

            _context.SaveChanges();

            _toastNotification.AddSuccessToastMessage($"Successfully deleted Appointment # {appointment.AppoitmentNumber}");
            return RedirectToAction(nameof(Index));
        }


        [HttpGet, ActionName("Delete")]        
        [Authorize(Policy = APP_POLICY_ADMIN.NAME)]
        public IActionResult DeleteView(int ID)
        {

            Appointment? appointment = _context.Appointment.Find(ID);

            if (appointment is null)
            {
                _toastNotification.AddErrorToastMessage($"Error : the Appointment #{ID} does not exists.");

                return RedirectToAction(nameof(Index));
            }

            return View(appointment);
        }

        [AcceptVerbs("GET","POST")]
        [Authorize(Policy = APP_POLICY_EDITABLE_CRUD.NAME)]
        public IActionResult IsNotRepeatedAppointmentNumber(string appointmentNumber)
        {

            bool isNOtRepeated = _context.Appointment
                                            .Where(ap => ap.AppoitmentNumber == appointmentNumber)
                                            .IsNullOrEmpty();
          
            return Json(isNOtRepeated);
        }

        public IActionResult TomorrowsAppointments()
        {
            var tomorrowsAppointments = _context.Appointment                                                
                                                .Include(appointment => appointment.Client)
                                                .Where(appointment => appointment.Date ==  DateOnly.FromDateTime(DateTime.Today.AddDays(1).Date))
                                                .ToList();
            return View(tomorrowsAppointments);
        }

        public IActionResult NextWeekAppointments()
        {
            var (startDate, endDate) = GetNextWeeksDates();

            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            List<Appointment> nextWeekAppointments = GetAppointmentsBetweenDates(startDate, endDate);
            return View(nextWeekAppointments);
        }

        private List<Appointment> GetAppointmentsBetweenDates(DateOnly startDate, DateOnly endDate)
        {
            return _context.Appointment      
                            .Include(appointment => appointment.Staff)                      
                            .Include(appointment => appointment.Client)
                            .Where(appointment => appointment.Date >= startDate && appointment.Date <= endDate)
                            .ToList();
        }

        private static   (DateOnly inicio, DateOnly fim) GetNextWeeksDates()
        {
                // Obtém a data atual
                DateTime hoje = DateTime.Now;

                // Calcula o próximo domingo (início da próxima semana)
                int diasAteDomingo = ((int)DayOfWeek.Sunday - (int)hoje.DayOfWeek + 7) % 7;
                DateTime proximoDomingo = hoje.AddDays(diasAteDomingo);

                // Define a data de início e fim da próxima semana
                DateTime inicioProximaSemana = proximoDomingo;
                DateTime fimProximaSemana = proximoDomingo.AddDays(6);
                // Retorna as datas como uma tupla
                return (DateOnly.FromDateTime(inicioProximaSemana.Date),DateOnly.FromDateTime(fimProximaSemana.Date));
        }
        public IActionResult EmailReminderNextWeeksAppointments()
        {
            // Obter data de início e fim da próxima semana
            var (startDate, endDate) = GetNextWeeksDates();



            // Obter as consultas da próxima semana
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            List<Appointment> nextWeekAppointments = GetAppointmentsBetweenDates(startDate, endDate);


            // Obter a língua e o respectivo template de e-mail
            var culture = Thread.CurrentThread.CurrentUICulture;

            string template = System.IO.File.ReadAllText(
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "EmailTemplates",
                    $"next_week_appointment.{culture.Name}.html"
                )
            );

            // Adaptar o template a cada consulta e enviar o e-mail
            foreach(var appointment in nextWeekAppointments)
            {
                StringBuilder htmlBody = new StringBuilder(template);
                htmlBody.Replace("##CUSTOMER_NAME##", appointment.Client.Name);
                htmlBody.Replace("##APPOINTMENT_DATE##", appointment.Date.ToShortDateString());
                htmlBody.Replace("##APPOINTMENT_TIME##", appointment.Time.ToShortTimeString());
               
                htmlBody.Replace("##STAFF_NAME##", appointment.Staff.Name);


                _emailSender.SendEmailAsync(appointment.Client.Email, "Reminder of Scheduled Appointment",
                    htmlBody.ToString());


            }
            
            _toastNotification.AddSuccessToastMessage($"{nextWeekAppointments.Count} Emails successfully sent.");


            return RedirectToAction(nameof(NextWeekAppointments));
        }

        
        private bool SendEmail (Appointment appointment) {
            
            Client? client = _context.Clients.Find(appointment.ClientID);
            Staff? staff = _context.Staff
                                        //.Include(s => s.StaffNumber)
                                        .Where(s => s.ID == appointment.StaffID)
                                        .Single();
                
            if (client == null || staff == null)
            {
                return false;
            }

            var culture = Thread.CurrentThread.CurrentUICulture;

            string template = System.IO.File.ReadAllText(
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "EmailTemplates",
                    $"create_appointment.{culture.Name}.html"
                )
            );

            StringBuilder htmlBody = new StringBuilder(template);
            htmlBody.Replace("##CUSTOMER_NAME##", client.Name);
            htmlBody.Replace("##APPOINTMENT_DATE##", appointment.Date.ToShortDateString());
            htmlBody.Replace("##APPOINTMENT_TIME##", appointment.Time.ToShortTimeString());
            htmlBody.Replace("##STAFF_ROLE##", staff.StaffNumber);
            htmlBody.Replace("##STAFF_NAME##", staff.Name);

            _emailSender.SendEmailAsync(client.Email, "Appointment Scheduled",
                    htmlBody.ToString());

            return false;
        }
    
    }


}

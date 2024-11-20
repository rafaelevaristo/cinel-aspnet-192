using mvc.Data;
using mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using NToastNotify;

using Microsoft.AspNetCore.Mvc.Localization;


namespace mvc.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly ILogger<AppointmentController> _logger;
        private readonly IToastNotification _toastNotification;

        private readonly ApplicationDbContext _context;
        private readonly IHtmlLocalizer<Resource> _sharedLocalizer ;

        public AppointmentController(ILogger<AppointmentController> logger,
                                    IToastNotification toastNotification,
                                    IHtmlLocalizer<Resource> localizer,
                                    ApplicationDbContext context)
        {
            _logger = logger;
            _toastNotification = toastNotification;
            _sharedLocalizer = localizer;
            _context = context;
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
        public IActionResult Create()
        {
            ViewBag.ClientList = new SelectList(_context.Clients, "ID", "Name");
            ViewBag.StaffList = new SelectList(_context.Staff, "ID", "Name");

            return View();
        }

        [HttpPost]
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
    }
}

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mvc.Data;
using mvc.Models;

namespace mvc.Controllers;

public class StaffController : Controller
{
    private readonly ILogger<StaffController> _logger;

    private readonly ApplicationDbContext _context;

    public StaffController(ILogger<StaffController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {        
        IEnumerable<Staff> clients = _context.Staff.ToList();              
        return View(clients);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Staff newStaff)
    {

        _context.Staff.Add(newStaff);

        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }


    public IActionResult Delete()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        Staff? staff = _context.Staff.Find(id);

        if (staff is null) 
        {
            return NotFound();
        } 
        
        return View(staff);
    }

    [HttpPost]
    public IActionResult Edit(Staff staffToUpdate)
    {

    //var dbClient = _context.Clients.Find(clientToUpdate.ID);

       
    try{

        _context.Staff.Update(staffToUpdate);
        _context.SaveChanges();

       }
       catch (Exception ex){
        //return NotFound();

        return BadRequest();

       }
       
       return RedirectToAction(nameof(Index));
    }


    [HttpGet]
    public IActionResult Details(int id)
    {
        Staff? staff = _context.Staff.Find(id);

        if (staff is null) 
        {
            return NotFound();
        } 

        return View(staff);
    }


    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirm(int ID)
    {

        Staff? staff = _context.Staff.Find(ID);

        if (staff is null) 
        {
            return NotFound();
        } 
        _context.Staff.Remove(staff);        

        _context.SaveChanges();
        
        return RedirectToAction(nameof(Index));
    }


    [HttpGet, ActionName("Delete")]
    public IActionResult DeleteView(int ID)
    {

        Staff? staff = _context.Staff.Find(ID);

        if (staff is null) 
        {
            return NotFound();
        } 

        return View(staff);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

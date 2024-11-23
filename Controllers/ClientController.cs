using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc.Data;
using mvc.Models;

namespace mvc.Controllers;

[Authorize]
public class ClientController : Controller
{
  
    private readonly ILogger<ClientController> _logger;

    private readonly ApplicationDbContext _context;

    public ClientController(ILogger<ClientController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {        
        IEnumerable<Client> clients = _context.Clients.ToList();              
        return View(clients);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Client newClient)
    {

        _context.Clients.Add(newClient);

        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirm(int ID)
    {

        Client? client = _context.Clients.Find(ID);

        if (client is null) 
        {
            return NotFound();
        } 
        _context.Clients.Remove(client);        

        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }


    [HttpGet, ActionName("Delete")]
    public IActionResult DeleteView(int ID)
    {

        Client? client = _context.Clients.Find(ID);

        if (client is null) 
        {
            return NotFound();
        } 

        return View(client);
    }


    [HttpGet]
    public IActionResult Edit(int id)
    {
        Client? client = _context.Clients.Find(id);

        if (client is null) 
        {
            return NotFound();
        } 
        
        return View(client);
    }

    [HttpPost]
    public IActionResult Edit(Client clientToUpdate)
    {

    //var dbClient = _context.Clients.Find(clientToUpdate.ID);

       
    try{

        _context.Clients.Update(clientToUpdate);
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
        Client? client = _context.Clients.Find(id);

        if (client is null) 
        {
            return NotFound();
        } 

        return View(client);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

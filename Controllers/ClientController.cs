using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mvc.Data;
using mvc.Models;

namespace mvc.Controllers;

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
        return View();
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


    public IActionResult Delete()
    {
        return View();
    }

    public IActionResult Update()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

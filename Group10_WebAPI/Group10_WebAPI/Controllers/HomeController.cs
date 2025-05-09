using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Group10_WebAPI.Models;

namespace Group10_WebAPI.Controllers;

public class HomeController : Controller
{
    // Home controller for Admin Views
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

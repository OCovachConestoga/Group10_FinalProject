using Group10_WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class GamesViewController : Controller
{
    // Simple Controller for Admin Views
    private readonly AppDbContext _context;
    public GamesViewController(AppDbContext context) { _context = context; }
    public IActionResult Index() => View("Index");
    [Authorize]
    public IActionResult Details(int id)
    {
        ViewData["GameId"] = id;
        return View("Details");
    }
    public IActionResult Create() => View("Create");
    [Authorize]
    public IActionResult Edit(int id)
    {
        ViewData["GameId"] = id;
        return View("Edit");
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        var game = _context.SpikeballGames.FirstOrDefault(g => g.GameId == id);
        if (game == null)
        {
            return NotFound("Game not found");
        }
        // Set the GameId in ViewData
        ViewData["GameId"] = game.GameId;
        return View(game); // Pass the game model to the view
    }

}

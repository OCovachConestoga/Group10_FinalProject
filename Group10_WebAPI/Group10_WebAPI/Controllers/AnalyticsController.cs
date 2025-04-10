using Group10_WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Group10_WebAPI.Controllers
{
    // Api Routes for the analytics functionalities
    // No Admin Views are included for this as this is mostly just a stats thing
    // THERE IS LOTS OF ROOM FOR SCALING AS THIS IS VERY BASIC STATS!!!
    [Route("api/analytics")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AnalyticsController(AppDbContext context) { _context = context; }

        // Counts total games played to get statistics of activity
        [Authorize]
        [HttpGet("games")]
        public IActionResult GetGameStats()
        {
            var stats = new { TotalGames = _context.SpikeballGames.Count() };
            return Ok(stats);
        }

        // Counts total users that have registered, to get stats of recognition of app
        [Authorize]
        [HttpGet("users")]
        public IActionResult GetUserStats()
        {
            var stats = new { TotalUsers = _context.Users.Count() };
            return Ok(stats);
        }

        // Counts common locations from multiple spikeball games
        [Authorize]
        [HttpGet("locations")]
        public IActionResult GetLocationTrends()
        {
            var locations = _context.SpikeballGames
                .GroupBy(g => g.Location)
                .Select(group => new { Location = group.Key, Count = group.Count() })
                .ToList();
            return Ok(locations);
        }
    }
}

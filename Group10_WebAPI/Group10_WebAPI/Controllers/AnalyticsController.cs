using Group10_WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Group10_WebAPI.Controllers
{
    [Route("api/analytics")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AnalyticsController(AppDbContext context) { _context = context; }

        [Authorize]
        [HttpGet("games")]
        public IActionResult GetGameStats()
        {
            var stats = new { TotalGames = _context.SpikeballGames.Count() };
            return Ok(stats);
        }

        [Authorize]
        [HttpGet("users")]
        public IActionResult GetUserStats()
        {
            var stats = new { TotalUsers = _context.Users.Count() };
            return Ok(stats);
        }

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

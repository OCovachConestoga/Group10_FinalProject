using Group10_WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [HttpGet("games")]
        public async Task<IActionResult> GetAllGames()
        {
            var groupedData = await _context.SpikeballGames
                .GroupBy(g => new { g.Date.Value.Year, g.Date.Value.Month })
                .Select(group => new
                {
                    group.Key.Year,
                    group.Key.Month,
                    GameCount = group.Count()
                })
                .ToListAsync(); // Fetch from DB first

            var result = groupedData
                .OrderByDescending(g => g.Year)
                .ThenByDescending(g => g.Month)
                .Select(g => new
                {
                    Month = $"{g.Year}-{g.Month:D2}", // Format here on client side
                    g.GameCount
                });

            return Ok(result);
        }

        // Counts total users that have registered, to get stats of recognition of app
        [HttpGet("users")]
        public async Task<IActionResult> GetUserStats()
        {
            var stats = await _context.Users.CountAsync();
            return Ok(stats);
        }

        // Counts common locations from multiple spikeball games
        [HttpGet("locations")]
        public async Task<IActionResult> GetLocationTrends()
        {
            // First, group by year, month, and location, then fetch the data.
            var groupedLocations = await _context.SpikeballGames
                .Where(g => g.Date.HasValue)
                .GroupBy(g => new
                {
                    g.Date.Value.Year,
                    g.Date.Value.Month,
                    g.Location
                })
                .Select(group => new
                {
                    group.Key.Year,
                    group.Key.Month,
                    group.Key.Location,
                    Count = group.Count()
                })
                .ToListAsync(); // Move this part to client-side evaluation to work with the data.

            // Now we can format the month-year string and group them again by month.
            var topLocationsPerMonth = groupedLocations
                .GroupBy(x => new { x.Year, x.Month })
                .SelectMany(g => g
                    .OrderByDescending(x => x.Count)
                    .Take(3)
                    .Select(x => new
                    {
                        Month = $"{g.Key.Month}-{g.Key.Year:D4}",
                        x.Location,
                        x.Count
                    }))
                .OrderBy(x => x.Month)
                .ToList();

            return Ok(topLocationsPerMonth);
        }
    }
}

using Group10_WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Group10_WebAPI.Controllers
{
    [Route("api/games")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly AppDbContext _context;
        public GameController(AppDbContext context) { _context = context; }

        [HttpPost("addGame")]
        [Authorize]
        public async Task<IActionResult> AddGame(Game game)
        {
            _context.SpikeballGames.Add(game);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGame), new { id = game.GameId }, game);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            return await _context.SpikeballGames.Where(g => !g.IsFull).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await _context.SpikeballGames.FindAsync(id);
            if (game == null) return NotFound();
            return game;
        }

        [HttpPost("{id}/join")]
        [Authorize]
        public async Task<IActionResult> JoinGame(int id, int userId)
        {
            var game = await _context.SpikeballGames.FindAsync(id);
            if (game == null || game.IsFull) return BadRequest("Game is full or not found");

            _context.GameResponses.Add(new GameResponse { GameId = id, UserId = userId, JoinedAt = DateTime.UtcNow });
            await _context.SaveChangesAsync();
            return Ok("Joined game");
        }

        [HttpPost("{id}/leave")]
        [Authorize]
        public async Task<IActionResult> LeaveGame(int id, int userId)
        {
            var response = await _context.GameResponses.FirstOrDefaultAsync(gr => gr.GameId == id && gr.UserId == userId);
            if (response == null) return NotFound("Not in game");
            _context.GameResponses.Remove(response);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [Authorize]
        [HttpGet("myGames")]
        public async Task<ActionResult<IEnumerable<Game>>> GetMyGames(int userId)
        {
            var games = await _context.GameResponses
                .Where(gr => gr.UserId == userId)
                .Join(_context.SpikeballGames,
                      gr => gr.GameId,
                      game => game.GameId,
                      (gr, game) => game) // Join GameResponse with SpikeballGame based on GameId
                .ToListAsync();

            if (!games.Any()) return NotFound("No games found");
            return games;
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("deleteGame/{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _context.SpikeballGames.FindAsync(id);
            if (game == null) return NotFound("Game not found");

            _context.SpikeballGames.Remove(game);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

using Group10_WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Group10_WebAPI.Controllers
{
    // API routes for games
    [Route("api/games")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly AppDbContext _context;
        public GameController(AppDbContext context) { _context = context; }

        [HttpPost("newPost")]
        [Authorize]
        public async Task<IActionResult> NewPost(Game game)
        {
            // Add the new game
            _context.SpikeballGames.Add(game);
            await _context.SaveChangesAsync();

            // Return the new game
            return CreatedAtAction(nameof(GetGame), new { id = game.GameId }, game);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            var games = await _context.SpikeballGames.ToListAsync();
            Console.WriteLine($"Returning {games.Count} games");  // Log the number of games
            return games;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await _context.SpikeballGames.FindAsync(id);
            if (game == null) return NotFound();
            return game;
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> EditGame(int id, Game updatedGame)
        {
            if (id != updatedGame.GameId)
                return BadRequest("Game ID mismatch");

            var existingGame = await _context.SpikeballGames.FindAsync(id);
            if (existingGame == null)
                return NotFound("Game not found");

            existingGame.Location = updatedGame.Location;
            existingGame.Date = updatedGame.Date;
            existingGame.MaxPlayers = updatedGame.MaxPlayers;
            existingGame.Organizer = updatedGame.Organizer;
            existingGame.IsFull = updatedGame.IsFull;

            _context.Entry(existingGame).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok("Game updated successfully");
        }

        [HttpPost("{id}/joinRequest")]
        [Authorize]
        public async Task<IActionResult> JoinGame(int id, string userId)
        {
            var game = await _context.SpikeballGames.FindAsync(id);
            if (game == null || game.IsFull) return BadRequest("Game is full or not found");

            _context.GameResponses.Add(new GameResponse { GameId = id, UserId = userId, JoinedAt = DateTime.UtcNow });
            await _context.SaveChangesAsync();
            return Ok("Joined game");
        }

        [HttpPost("{id}/leaveRequest")]
        [Authorize]
        public async Task<IActionResult> LeaveGame(int id, string userId)
        {
            var response = await _context.GameResponses.FirstOrDefaultAsync(gr => gr.GameId == id && gr.UserId == userId);
            if (response == null) return NotFound("Not in game");
            _context.GameResponses.Remove(response);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [Authorize]
        [HttpGet("myGames")]
        public async Task<ActionResult<IEnumerable<Game>>> GetMyGames(string userId)
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
        [Authorize (Roles = "Admin")]
        [HttpPost("deleteGame/{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            // Find the game by id
            var game = await _context.SpikeballGames.FindAsync(id);

            // If game not found, return a NotFound response
            if (game == null)
            {
                return NotFound("Game not found");
            }

            // Remove the game from the database
            _context.SpikeballGames.Remove(game);
            await _context.SaveChangesAsync();

            // Redirect to the index page or another page after successful deletion
            return RedirectToAction("Index", "GamesView");
        }
    }
}

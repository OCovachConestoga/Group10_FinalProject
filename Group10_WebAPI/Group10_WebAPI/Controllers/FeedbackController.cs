using Group10_WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Group10_WebAPI.Controllers
{
    [Route("api/feedback")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly AppDbContext _context;
        public FeedbackController(AppDbContext context) { _context = context; }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SubmitFeedback(Feedback feedback)
        {
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
            return Ok("Feedback submitted successfully");
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetAllFeedback()
        {
            return await _context.Feedbacks.ToListAsync();
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Group10_WebAPI.Controllers
{
    // Simple Controller to allow for Admin Views
    public class FeedbackViewController : Controller
    {
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View("Index");
        }
    }
}

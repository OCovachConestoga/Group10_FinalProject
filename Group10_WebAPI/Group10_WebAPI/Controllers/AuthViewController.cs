using Group10_WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Group10_WebAPI.Controllers
{
    // Controller responsible for the Admin Views
    public class AuthViewController : Controller
    {
        private readonly AppDbContext _context;

        public AuthViewController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(string id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id); // Find user
            if (user == null)
            {
                return NotFound(); // Return 404 if the user isn't found
            }
            return View("Edit", user);  // Return the EditUser view with the user data
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id); // Find user
            if (user == null)
            {
                return NotFound(); // Return 404 if the user isn't found
            }
            return View("Delete", user); // Return the DeleteUser view with the user data
        }

        public IActionResult Login()
        {
            return View("Login");
        }

        public IActionResult Register()
        {
            return View("Register");
        }

        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }
    }
}

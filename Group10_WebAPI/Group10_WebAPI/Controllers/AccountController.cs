using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Group10_WebAPI.Controllers
{
    // Simple controller to handle the *unauthorized access* to pages or actions
    // All of these just redirect to the AuthView Controller that I made (That way this program follows our design to the best ability)
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return RedirectToAction("AccessDenied", "AuthView");
        }

        public IActionResult Login()
        {
            return RedirectToAction("Login", "AuthView");
        }

        public IActionResult Register()
        {
            return RedirectToAction("Register", "AuthView");
        }
    }
}

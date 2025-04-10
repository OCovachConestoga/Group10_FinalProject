using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Group10_WebAPI.Controllers
{
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

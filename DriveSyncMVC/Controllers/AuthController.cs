using Microsoft.AspNetCore.Mvc;
using DriveSyncMVC.Services;

namespace DriveSyncMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _auth;

        public AuthController(AuthService auth)
        {
            _auth = auth;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var result = await _auth.LoginAsync(email, password);

            if (!result.Success)
            {
                ViewBag.Error = "Invalid login";
                return View();
            }

            // ✅ SAVE SESSION (IMPORTANT)
            HttpContext.Session.SetString("role", result.Role);
            HttpContext.Session.SetString("name", result.Name);

            // 👑 Admin redirect
            if (result.Role == "Admin" || result.Role == "Owner")
                return RedirectToAction("Index", "Admin");

            // 👤 User dashboard
            return RedirectToAction("Index", "User");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string name, string email, string password, string role)
        {
            var success = await _auth.RegisterAsync(name, email, password, role);

            if (!success)
            {
                ViewBag.Error = "Registration failed";
                return View();
            }

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            // ❗ Clear BOTH cookie + session
            Response.Cookies.Delete("jwt");
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
    }
}
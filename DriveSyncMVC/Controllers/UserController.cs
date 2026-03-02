using Microsoft.AspNetCore.Mvc;

namespace DriveSyncMVC.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
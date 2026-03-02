using Microsoft.AspNetCore.Mvc;

namespace DriveSyncMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
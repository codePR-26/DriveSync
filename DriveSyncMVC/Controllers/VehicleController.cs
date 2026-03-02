using Microsoft.AspNetCore.Mvc;
using DriveSyncMVC.Services;

namespace DriveSyncMVC.Controllers
{
    public class VehicleController : Controller
    {
        private readonly VehicleService _vehicleService;

        public VehicleController(VehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public async Task<IActionResult> Index()
        {
            var vehicles = await _vehicleService.GetAllVehiclesAsync();
            return View(vehicles);
        }
    }
}
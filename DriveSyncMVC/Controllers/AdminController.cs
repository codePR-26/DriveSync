using Microsoft.AspNetCore.Mvc;
using DriveSyncMVC.Services;
using System.Net.Http.Json;

namespace DriveSyncMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly VehicleService _vehicle;
        private readonly IHttpClientFactory _factory;
        private readonly IHttpContextAccessor _http;

        public AdminController(
            VehicleService vehicle,
            IHttpClientFactory factory,
            IHttpContextAccessor http)
        {
            _vehicle = vehicle;
            _factory = factory;
            _http = http;
        }

        // 👑 DASHBOARD
        public IActionResult Index()
        {
            return View();
        }

        // 🚗 ALL VEHICLES
        public async Task<IActionResult> Vehicles()
        {
            var list = await _vehicle.GetAllVehiclesAsync();
            return View(list);
        }

        // ➕ ADD VEHICLE PAGE
        public IActionResult AddVehicle()
        {
            return View();
        }

        // ➕ ADD VEHICLE POST
        [HttpPost]
        public async Task<IActionResult> AddVehicle(
            string brand,
            string model,
            int passengerCapacity,
            int engineCapacity,
            decimal dailyRate,
            decimal monthlyRate)
        {
            var success = await _vehicle.AddVehicleAsync(new
            {
                brand,
                model,
                passengerCapacity,
                engineCapacity,
                dailyRate,
                monthlyRate
            });

            if (!success)
            {
                ViewBag.Error = "Failed to add vehicle";
                return View();
            }

            return RedirectToAction("Vehicles");
        }

        // 📊 BOOKINGS HISTORY (FIXED VERSION)
        public async Task<IActionResult> Bookings()
        {
            var client = _factory.CreateClient("DriveSyncAPI");

            // 🔥 SAFELY FORWARD COOKIE
            var cookieHeader = _http.HttpContext?.Request.Headers["Cookie"].ToString();

            if (!string.IsNullOrEmpty(cookieHeader))
            {
                if (!client.DefaultRequestHeaders.Contains("Cookie"))
                    client.DefaultRequestHeaders.Add("Cookie", cookieHeader);
            }

            try
            {
                var data = await client.GetFromJsonAsync<List<AdminBookingDto>>("Admin/reservations");
                return View(data ?? new List<AdminBookingDto>());
            }
            catch
            {
                // 🔥 Prevent crash if API returns 401/500
                ViewBag.Error = "Failed to load bookings (try login again)";
                return View(new List<AdminBookingDto>());
            }
        }
    }
}
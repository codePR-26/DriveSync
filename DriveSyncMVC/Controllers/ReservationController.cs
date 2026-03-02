using Microsoft.AspNetCore.Mvc;
using DriveSyncMVC.Services;

namespace DriveSyncMVC.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ReservationService _service;
        private readonly IHttpClientFactory _factory;
        private readonly IHttpContextAccessor _http;

        public ReservationController(
            ReservationService service,
            IHttpClientFactory factory,
            IHttpContextAccessor http)
        {
            _service = service;
            _factory = factory;
            _http = http;
        }

        // ===============================
        // BOOKING PAGE
        // ===============================
        public IActionResult Create(int vehicleId)
        {
            ViewBag.VehicleId = vehicleId;
            return View();
        }

        // ===============================
        // CREATE BOOKING
        // ===============================
        [HttpPost]
        public async Task<IActionResult> Create(int vehicleId, DateTime startDate, DateTime endDate)
        {
            var success = await _service.CreateReservation(vehicleId, startDate, endDate);

            if (!success)
            {
                ViewBag.VehicleId = vehicleId;
                ViewBag.Error = "Booking failed";
                return View();
            }

            return RedirectToAction("My");
        }

        // ===============================
        // MY RESERVATIONS PAGE
        // ===============================
        public async Task<IActionResult> My()
        {
            var list = await _service.GetMyReservations();
            return View(list);
        }

        // ===============================
        // CANCEL BOOKING
        // ===============================
        public async Task<IActionResult> Cancel(int id)
        {
            var client = _factory.CreateClient("DriveSyncAPI");

            // forward cookies
            var cookies = _http.HttpContext.Request.Headers["Cookie"].ToString();
            if (!string.IsNullOrEmpty(cookies))
                client.DefaultRequestHeaders.Add("Cookie", cookies);

            await client.PatchAsync($"Reservation/cancel/{id}", null);

            TempData["Success"] = "Reservation created successfully!";
            return RedirectToAction("My");
        }
    }
}
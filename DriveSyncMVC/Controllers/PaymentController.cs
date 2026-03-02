using Microsoft.AspNetCore.Mvc;
using DriveSyncMVC.Services;

namespace DriveSyncMVC.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IHttpClientFactory _factory;
        private readonly IHttpContextAccessor _http;

        public PaymentController(IHttpClientFactory factory, IHttpContextAccessor http)
        {
            _factory = factory;
            _http = http;
        }

        public async Task<IActionResult> Pay(int reservationId)
        {
            var client = _factory.CreateClient("DriveSyncAPI");

            // forward cookies
            var cookies = _http.HttpContext.Request.Headers["Cookie"].ToString();
            if (!string.IsNullOrEmpty(cookies))
                client.DefaultRequestHeaders.Add("Cookie", cookies);

            var response = await client.PostAsJsonAsync("Payment", new
            {
                reservationId = reservationId,
                paymentMethod = "UPI"
            });

            return RedirectToAction("My", "Reservation");
        }
    }
}
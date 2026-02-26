using DriveSync.Data;
using DriveSync.DTOs.Payment;
using DriveSync.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DriveSync.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class PaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaymentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> MakePayment(CreatePaymentDto dto)
        {
            var reservation =
                await _context.Reservations
                .FirstOrDefaultAsync(r =>
                    r.ReservationId == dto.ReservationId);

            if (reservation == null)
                return NotFound("Reservation Not Found");

            if (reservation.Status == "Confirmed")
                return BadRequest("Already Paid");

            var payment = new Payment
            {
                ReservationId = reservation.ReservationId,
                Amount = reservation.TotalCost,
                PaymentMethod = dto.PaymentMethod,
                PaymentStatus = "Paid",
                TransactionId = Guid.NewGuid().ToString()
            };

            _context.Payments.Add(payment);

            // 🔥 IMPORTANT
            reservation.Status = "Confirmed";

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Payment Successful",
                reservation.Status
            });
        }
    }
}
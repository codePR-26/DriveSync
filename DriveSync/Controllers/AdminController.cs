using DriveSync.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DriveSync.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    // Admin + Owner access
    [Authorize(Roles = "Admin,Owner")]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminController(
            ApplicationDbContext context)
        {
            _context = context;
        }


        // ===============================
        // VIEW ALL RESERVATIONS
        // ===============================

        [HttpGet("reservations")]
        public async Task<IActionResult>
            GetAllReservations()
        {
            var data =
                await _context.Reservations

                .Include(r => r.Vehicle)
                .Include(r => r.Customer)
                .Include(r => r.Payments)

                .Select(r => new
                {
                    r.ReservationId,

                    CustomerName =
                        r.Customer.Name,

                    Vehicle =
                        r.Vehicle.Model,

                    r.StartDate,

                    r.EndDate,

                    r.TotalCost,

                    r.Status,

                    Payment =
                        r.Payments
                        .Select(p => p.PaymentStatus)
                        .FirstOrDefault()
                })

                .ToListAsync();

            return Ok(data);
        }
    }
}
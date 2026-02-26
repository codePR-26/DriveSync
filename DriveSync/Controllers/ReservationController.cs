using DriveSync.Data;
using DriveSync.DTOs.Reservation;
using DriveSync.Models;
using DriveSync.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DriveSync.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    // Only Customer can access these APIs
    [Authorize(Roles = "Customer")]
    public class ReservationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ReservationService _reservationService;

        public ReservationController(
            ApplicationDbContext context,
            ReservationService reservationService)
        {
            _context = context;
            _reservationService = reservationService;
        }


        // ======================================================
        // CREATE RESERVATION
        // TYPE : POST
        // ENDPOINT : POST api/reservation
        // PURPOSE : Customer books a vehicle
        // ======================================================

        [HttpPost]
        public async Task<IActionResult> CreateReservation(
            CreateReservationDto dto)
        {
            // ===============================
            // GET CUSTOMER FROM JWT TOKEN
            // ===============================

            var userIdValue = User.FindFirstValue("UserId");

            if (string.IsNullOrEmpty(userIdValue))
                return Unauthorized("Invalid Token");

            if (!int.TryParse(userIdValue, out int customerId))
                return Unauthorized("Invalid UserId");


            // ===============================
            // DATE VALIDATION
            // ===============================

            if (dto.EndDate <= dto.StartDate)
                return BadRequest("End Date must be greater than Start Date");


            // ===============================
            // CHECK VEHICLE EXISTS
            // ===============================

            var vehicle =
                await _context.Vehicles
                .FirstOrDefaultAsync(
                    v => v.VehicleId == dto.VehicleId);

            if (vehicle == null)
                return NotFound("Vehicle Not Found");


            // ===============================
            // VEHICLE AVAILABILITY CHECK
            // ===============================

            var available =
                await _reservationService
                .IsVehicleAvailable(
                    dto.VehicleId,
                    dto.StartDate,
                    dto.EndDate);

            if (!available)
                return BadRequest("Vehicle already reserved for selected dates");


            // ===============================
            // COST CALCULATION
            // ===============================

            var totalDays =
                (dto.EndDate - dto.StartDate).Days;

            if (totalDays <= 0)
                totalDays = 1;

            var totalCost =
                totalDays * vehicle.DailyRate;


            // ===============================
            // CREATE RESERVATION
            // ===============================

            var reservation = new Reservation
            {
                CustomerId = customerId,
                VehicleId = dto.VehicleId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                TotalCost = totalCost,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.Reservations.Add(reservation);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Reservation Created Successfully",
                reservation.ReservationId,
                reservation.TotalCost
            });
        }



        // ======================================================
        // GET MY RESERVATIONS
        // TYPE : GET
        // ENDPOINT : GET api/reservation/my
        // PURPOSE : Customer dashboard bookings view
        // ======================================================

        [HttpGet("my")]
        public async Task<IActionResult> GetMyReservations()
        {
            var userIdValue = User.FindFirstValue("UserId");

            if (string.IsNullOrEmpty(userIdValue))
                return Unauthorized("Invalid Token");

            if (!int.TryParse(userIdValue, out int customerId))
                return Unauthorized("Invalid UserId");


            var reservations =
                await _context.Reservations

                .Include(r => r.Vehicle)
                .Include(r => r.Payments)

                .Where(r => r.CustomerId == customerId)

                .Select(r => new
                {
                    r.ReservationId,

                    VehicleModel = r.Vehicle.Model,

                    Brand = r.Vehicle.Brand,

                    r.StartDate,

                    r.EndDate,

                    r.TotalCost,

                    r.Status,

                    PaymentStatus =
                        r.Payments
                        .OrderByDescending(p => p.PaymentDate)
                        .Select(p => p.PaymentStatus)
                        .FirstOrDefault()
                })

                .ToListAsync();

            return Ok(reservations);
        }



        // ======================================================
        // CANCEL RESERVATION
        // TYPE : PATCH
        // ENDPOINT : PATCH api/reservation/cancel/{id}
        // PURPOSE : Customer cancels booking
        // ======================================================

        [HttpPatch("cancel/{id}")]
        public async Task<IActionResult> CancelReservation(int id)
        {
            var userIdValue = User.FindFirstValue("UserId");

            if (string.IsNullOrEmpty(userIdValue))
                return Unauthorized("Invalid Token");

            if (!int.TryParse(userIdValue, out int customerId))
                return Unauthorized("Invalid UserId");


            var reservation =
                await _context.Reservations
                .FirstOrDefaultAsync(r =>
                    r.ReservationId == id);

            if (reservation == null)
                return NotFound("Reservation Not Found");


            // SECURITY CHECK
            if (reservation.CustomerId != customerId)
                return Unauthorized("Not Your Reservation");


            if (reservation.Status == "Cancelled")
                return BadRequest("Already Cancelled");


            reservation.Status = "Cancelled";

            await _context.SaveChangesAsync();

            return Ok("Reservation Cancelled Successfully");
        }
    }
}
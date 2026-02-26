using DriveSync.Data;
using DriveSync.Models;
using Microsoft.EntityFrameworkCore;

namespace DriveSync.Services
{
    public class ReservationService
    {
        private readonly ApplicationDbContext _context;

        public ReservationService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Vehicle availability check
        public async Task<bool> IsVehicleAvailable(
            int vehicleId,
            DateTime start,
            DateTime end)
        {
            return !await _context.Reservations
                .AnyAsync(r =>

                    r.VehicleId == vehicleId &&

                    r.Status != "Cancelled" &&

                    r.StartDate <= end &&
                    r.EndDate >= start
                );
        }
    }
}
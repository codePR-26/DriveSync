using System.ComponentModel.DataAnnotations;

namespace DriveSync.DTOs.Reservation
{
    public class CreateReservationDto
    {
        [Required]
        public int VehicleId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}
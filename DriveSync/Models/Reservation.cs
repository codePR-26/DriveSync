using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DriveSync.Constants;

namespace DriveSync.Models
{
    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Account Customer { get; set; }

        [Required]
        public int VehicleId { get; set; }

        [ForeignKey("VehicleId")]
        public Vehicle Vehicle { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public decimal TotalCost { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = ReservationStatuses.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Payment>? Payments { get; set; }
    }
}
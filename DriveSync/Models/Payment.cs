using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DriveSync.Constants;

namespace DriveSync.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        // FK Reservation
        [Required]
        public int ReservationId { get; set; }

        [ForeignKey("ReservationId")]
        public Reservation Reservation { get; set; }

        [Required]
        public decimal Amount { get; set; }

        // UPI / Card / Cash etc
        [Required]
        [MaxLength(50)]
        public string PaymentMethod { get; set; }

        [Required]
        [MaxLength(20)]
        public string PaymentStatus { get; set; }
            = PaymentStatuses.Pending;

        // Gateway transaction id later
        public string? TransactionId { get; set; }

        public DateTime PaymentDate { get; set; }
            = DateTime.UtcNow;
    }
}
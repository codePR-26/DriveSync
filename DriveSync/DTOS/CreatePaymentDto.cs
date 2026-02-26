using System.ComponentModel.DataAnnotations;

namespace DriveSync.DTOs.Payment
{
    public class CreatePaymentDto
    {
        [Required]
        public int ReservationId { get; set; }

        [Required]
        public string PaymentMethod { get; set; }
    }
}
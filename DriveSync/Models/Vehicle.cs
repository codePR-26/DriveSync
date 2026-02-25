using System.ComponentModel.DataAnnotations;

namespace DriveSync.Models
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Model { get; set; }

        public int Year { get; set; }

        [Required]
        public decimal PricePerDay { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
}
using System.ComponentModel.DataAnnotations;

namespace DriveSync.DTOS
{
    public class UpdateVehicleRequestDto
    {
        [Required]
        public string Model { get; set; }

        [Required]
        public string Brand { get; set; }

        public int PassengerCapacity { get; set; }

        public int EngineCapacity { get; set; }

        public decimal DailyRate { get; set; }

        public decimal MonthlyRate { get; set; }

        public string Status { get; set; } 
    }
}

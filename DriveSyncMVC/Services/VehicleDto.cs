namespace DriveSyncMVC.Services
{
    
        // Simple DTO for MVC
        public class VehicleDto
        {
            public int VehicleId { get; set; }
            public string Brand { get; set; }
            public string Model { get; set; }
            public decimal DailyRate { get; set; }
            public int PassengerCapacity { get; set; }
            public string Status { get; set; }
        }
    }

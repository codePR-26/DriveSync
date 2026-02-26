using DriveSync.Models;
using System.Collections.Generic;  
using System.ComponentModel.DataAnnotations;

public class Vehicle
{
    [Key]
    public int VehicleId { get; set; }

    [Required]
    public string Model { get; set; }

    [Required]
    public string Brand { get; set; }

    public int PassengerCapacity { get; set; }

    public int EngineCapacity { get; set; }

    public decimal DailyRate { get; set; }

    public decimal MonthlyRate { get; set; }

    public string Status { get; set; } = "Available";

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Navigation Property
    // One vehicle → many reservations
    public ICollection<Reservation>? Reservations { get; set; }
}
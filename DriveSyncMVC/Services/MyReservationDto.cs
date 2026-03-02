namespace DriveSyncMVC.Services
{
    public class MyReservationDto
    {
        public int ReservationId { get; set; }
        public string VehicleModel { get; set; }
        public string Brand { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalCost { get; set; }
        public string Status { get; set; }
        public string? PaymentStatus { get; set; }
    }
}
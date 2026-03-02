namespace DriveSyncMVC.Services
{
    public class AdminBookingDto
    {
        public int ReservationId { get; set; }
        public string CustomerName { get; set; }
        public string Vehicle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalCost { get; set; }
        public string Status { get; set; }
        public string Payment { get; set; }
    }
}
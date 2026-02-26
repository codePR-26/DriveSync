using System.ComponentModel.DataAnnotations;

namespace DriveSync.Models
{
    // This model stores login accounts
    // Owner = Parent Admin
    // Admin = System Manager
    // Customer = Normal User

    public class Account
    {
        // Auto identity primary key
        public int Id { get; set; }

        // Person full name
        [Required]
        public string Name { get; set; }

        // Login email (must be unique)
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // Plain password (simple project version)
        [Required]
        public string Password { get; set; }

        // Store readable role text in database
        // Owner / Admin / Customer

        public string? Role { get; set; }


        public ICollection<Reservation>? Reservations { get; set; }
    }
}
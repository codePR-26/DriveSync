using System.ComponentModel.DataAnnotations;

namespace DriveSync.DTOs
{
    // Only login fields
    public class LoginDTOs
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
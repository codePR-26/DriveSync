using System.ComponentModel.DataAnnotations;

namespace DriveSync.DTOS
{
    public class RegisterUserRequestDto
    {
        [Required]
        public string Name { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

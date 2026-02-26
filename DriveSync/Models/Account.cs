using System.ComponentModel.DataAnnotations;

namespace DriveSync.Models
{
    public enum Role
    {
        User,
        Admin,
        ParentAdmin
    }

    public class Account
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public Role Role { get; set; }
    }
}
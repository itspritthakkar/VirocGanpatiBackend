using System.ComponentModel.DataAnnotations;

namespace VirocGanpati.DTOs
{
    public class AddUserDto
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?:\+91|0)?[6-9]\d{9}$", ErrorMessage = "Invalid mobile number format.")]
        public string Mobile { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Status { get; set; }

        // RoleId is mandatory to assign the user a role
        [Required]
        public int RoleId { get; set; }
        [Required]
        public int MandalId { get; set; }
    }

}

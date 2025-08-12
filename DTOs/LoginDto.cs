using System.ComponentModel.DataAnnotations;

namespace VirocGanpati.DTOs
{
    public class LoginDto
    {
        [Required]
        [RegularExpression(@"^(?:\+91|0)?[6-9]\d{9}$", ErrorMessage = "Invalid mobile number format.")]
        public string Mobile { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

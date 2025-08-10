using System.ComponentModel.DataAnnotations;

namespace VirocGanpati.DTOs
{
    public class UpdateManagerDto
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

        public string? Password { get; set; }
    }
}

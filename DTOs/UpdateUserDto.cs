using System.ComponentModel.DataAnnotations;

namespace VirocGanpati.DTOs
{
    public class UpdateUserDto
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
        public string Status { get; set; }

        [Required]
        public int RoleId { get; set; }  // Link to Role

        [Required]
        public int ProjectId { get; set; }

        public string? Password { get; set; }
    }
}

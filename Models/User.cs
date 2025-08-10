using System.ComponentModel.DataAnnotations;

namespace VirocGanpati.Models
{

    public class User
    {
        [Key]
        public int UserId { get; set; }

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
        public bool IsMobileVerified { get; set; } = false;

        [Required]
        public string Password { get; set; }  // Hashed password

        [Required]
        public ActiveInactiveStatus Status { get; set; }

        // Foreign Key to Role
        public int RoleId { get; set; }
        public Role Role { get; set; }  // Navigation property

        // Nullable Foreign Key to Mandal
        public int? MandalId { get; set; }
        public Mandal Mandal { get; set; }  // Navigation property
        public DateTime? LastLoginAt { get; set; }  // Stores the last login timestamp

        // Soft delete
        public bool IsDeleted { get; set; } = false;

        // Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }

}

using System.ComponentModel.DataAnnotations;

namespace VirocGanpati.Models
{
    public class OtpMessage
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string MobileNumber { get; set; }

        [Required]
        [MaxLength(6)]
        public string OtpCode { get; set; }

        [Required]
        [MaxLength(50)]
        public string Purpose { get; set; } // e.g., Signup, ForgotPassword

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiryAt { get; set; }
        public bool IsUsed { get; set; } = false;
    }
}

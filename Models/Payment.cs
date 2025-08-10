using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VirocGanpati.Enums;

namespace VirocGanpati.Models
{
    public class Payment
    {
        [Key]
        public Guid PaymentId { get; set; } = Guid.NewGuid();

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
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(255)]
        public string MandalName { get; set; }

        [StringLength(1000)]
        public string MandalDescription { get; set; }

        // Foreign Keys
        public int AreaId { get; set; }
        public MandalArea Area { get; set; }

        public int ArtiMorningTimeId { get; set; }
        public ArtiMorningTime ArtiMorningTime { get; set; }

        public int ArtiEveningTimeId { get; set; }
        public ArtiEveningTime ArtiEveningTime { get; set; }

        public int DateOfVisarjanId { get; set; }
        public DateOfVisarjan DateOfVisarjan { get; set; }

        public string RazorpayOrderId { get; set; }

        public string? RazorpayPaymentId { get; set; }

        public string? RazorpaySignature { get; set; }

        public PaymentStatus Status { get; set; } = PaymentStatus.Initiated;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

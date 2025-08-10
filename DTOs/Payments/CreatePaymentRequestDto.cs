using System.ComponentModel.DataAnnotations;

namespace VirocGanpati.DTOs.Payments
{
    public class CreatePaymentRequestDto
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
        [StringLength(255)]
        public string MandalName { get; set; }

        [StringLength(1000)]
        public string MandalDescription { get; set; }

        [Required]
        public int AreaId { get; set; }

        [Required]
        public int ArtiMorningTimeId { get; set; }

        [Required]
        public int ArtiEveningTimeId { get; set; }

        [Required]
        public int DateOfVisarjanId { get; set; }
    }
}

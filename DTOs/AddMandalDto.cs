using System.ComponentModel.DataAnnotations;

namespace VirocGanpati.DTOs
{
    public class AddMandalDto
    {
        [Required]
        [StringLength(255)]
        public string MandalName { get; set; }

        [Required]
        public string Status { get; set; }

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

        public string? CreatedBy { get; set; }
    }
}

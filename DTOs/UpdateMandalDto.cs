using System.ComponentModel.DataAnnotations;

namespace VirocGanpati.DTOs
{
    public class UpdateMandalDto
    {
        [Required]
        [StringLength(255)]
        public string ProjectName { get; set; }

        [Required]
        public string Status { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }
    }
}

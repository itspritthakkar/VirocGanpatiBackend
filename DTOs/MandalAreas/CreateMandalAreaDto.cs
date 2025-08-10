using System.ComponentModel.DataAnnotations;

namespace VirocGanpati.DTOs.MandalAreas
{
    public class CreateMandalAreaDto
    {
        [Required]
        [StringLength(100)]
        public string AreaName { get; set; }
    }
}

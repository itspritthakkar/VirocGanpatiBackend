using System.ComponentModel.DataAnnotations;

namespace VirocGanpati.DTOs.MandalAreas
{
    public class UpdateMandalAreaDto
    {
        public int AreaId { get; set; }

        [Required]
        [StringLength(100)]
        public string AreaName { get; set; }
    }
}

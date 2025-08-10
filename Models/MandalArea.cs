using System.ComponentModel.DataAnnotations;

namespace VirocGanpati.Models
{
    public class MandalArea
    {
        [Key]
        public int AreaId { get; set; }

        [Required]
        [StringLength(100)]
        public string AreaName { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace VirocGanpati.Models
{
    public class ArtiMorningTime
    {
        [Key]
        public int ArtiMorningTimeId { get; set; }

        [Required]
        public string Value { get; set; }
    }
}

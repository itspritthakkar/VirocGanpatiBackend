using System.ComponentModel.DataAnnotations;

namespace VirocGanpati.Models
{
    public class ArtiEveningTime
    {
        [Key]
        public int ArtiEveningTimeId { get; set; }

        [Required]
        public string Value { get; set; }
    }
}

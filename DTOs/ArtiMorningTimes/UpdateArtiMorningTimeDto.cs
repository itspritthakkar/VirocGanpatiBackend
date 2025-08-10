using System.ComponentModel.DataAnnotations;

namespace VirocGanpati.DTOs.ArtiMorningTimes
{
    public class UpdateArtiMorningTimeDto
    {
        public int ArtiMorningTimeId { get; set; }

        [Required]
        public string Value { get; set; }
    }
}

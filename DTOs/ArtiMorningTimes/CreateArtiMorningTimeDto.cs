using System.ComponentModel.DataAnnotations;

namespace VirocGanpati.DTOs.ArtiMorningTimes
{
    public class CreateArtiMorningTimeDto
    {
        [Required]
        public string Value { get; set; }
    }
}

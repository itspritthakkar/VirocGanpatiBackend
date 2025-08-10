using System.ComponentModel.DataAnnotations;

namespace VirocGanpati.DTOs.ArtiEveningTime
{
    public class CreateArtiEveningTimeDto
    {
        [Required]
        public string Value { get; set; }
    }
}

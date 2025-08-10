using System.ComponentModel.DataAnnotations;

namespace VirocGanpati.DTOs.ArtiEveningTime
{
    public class UpdateArtiEveningTimeDto
    {
        public int ArtiEveningTimeId { get; set; }

        [Required]
        public string Value { get; set; }
    }
}

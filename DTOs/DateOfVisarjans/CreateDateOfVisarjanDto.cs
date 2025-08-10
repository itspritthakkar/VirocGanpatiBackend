using System.ComponentModel.DataAnnotations;

namespace VirocGanpati.DTOs.DateOfVisarjans
{
    public class CreateDateOfVisarjanDto
    {
        [Required]
        public string Value { get; set; }
    }
}

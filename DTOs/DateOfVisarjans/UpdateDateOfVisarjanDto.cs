using System.ComponentModel.DataAnnotations;

namespace VirocGanpati.DTOs.DateOfVisarjans
{
    public class UpdateDateOfVisarjanDto
    {
        public int DateOfVisarjanId { get; set; }

        [Required]
        public string Value { get; set; }
    }
}

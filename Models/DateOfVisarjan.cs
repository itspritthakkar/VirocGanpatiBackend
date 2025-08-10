using System.ComponentModel.DataAnnotations;

namespace VirocGanpati.Models
{
    public class DateOfVisarjan
    {
        [Key]
        public int DateOfVisarjanId { get; set; }

        [Required]
        public string Value { get; set; }  // Could be a string like "28-Aug-2025" or use DateTime
    }
}

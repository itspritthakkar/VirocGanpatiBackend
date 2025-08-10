using System.ComponentModel.DataAnnotations;

namespace VirocGanpati.Models
{
    public class Mandal
    {
        [Key]
        public int MandalId { get; set; }

        [Required]
        [StringLength(255)]
        public string MandalName { get; set; }

        [Required]
        [StringLength(255)]
        [RegularExpression(@"^[a-z0-9_]+$", ErrorMessage = "MandalSlug can only contain lowercase letters, numbers, and underscores.")]
        public string MandalSlug { get; set; }

        [Required]
        public ActiveInactiveStatus Status { get; set; }

        [StringLength(1000)]
        public string MandalDescription { get; set; }

        // Foreign Keys
        public int AreaId { get; set; }
        public MandalArea Area { get; set; }

        public int ArtiMorningTimeId { get; set; }
        public ArtiMorningTime ArtiMorningTime { get; set; }

        public int ArtiEveningTimeId { get; set; }
        public ArtiEveningTime ArtiEveningTime { get; set; }

        public int DateOfVisarjanId { get; set; }
        public DateOfVisarjan DateOfVisarjan { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public User? Updater { get; set; }
    }
}

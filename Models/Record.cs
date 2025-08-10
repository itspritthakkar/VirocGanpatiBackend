using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace VirocGanpati.Models
{
    [Index(nameof(CreatedAt))]
    public class Record
    {
        [Key]
        public int RecordId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public User Updater { get; set; }

        public int MandalId { get; set; }
        public Mandal Mandal { get; set; }

        public ICollection<Document> Documents { get; set; }
    }
}

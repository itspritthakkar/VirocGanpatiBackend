using System.ComponentModel.DataAnnotations;

namespace VirocGanpati.Models
{
    public class Document
    {
        [Key]
        public int DocumentId { get; set; }

        // Foreign key to Record
        public int RecordId { get; set; }
        public Record Record { get; set; }

        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string? Description { get; set; }

        // Generic fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}

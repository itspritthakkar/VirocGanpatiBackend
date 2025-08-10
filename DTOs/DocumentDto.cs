namespace VirocGanpati.DTOs
{
    public class DocumentDto
    {
        public int DocumentId { get; set; }
        public int RecordId { get; set; }
        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string? FileKey { get; set; }
        public string? FileUrl { get; set; }
        public bool IsCompressed { get; set; }
        public string? CompressedFileKey { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

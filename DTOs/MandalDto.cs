namespace VirocGanpati.DTOs
{
    public class MandalDto
    {
        public int MandalId { get; set; }
        public string MandalName { get; set; }
        public string MandalSlug { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string MandalDescription { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
    }
}



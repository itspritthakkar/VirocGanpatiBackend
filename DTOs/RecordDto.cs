namespace VirocGanpati.DTOs
{
    public class RecordDto
    {
        public int RecordId { get; set; }
        public string TenementNo { get; set; }
        public int WardNo { get; set; }
        public string ProjectName { get; set; }
        public string OldTenementNo { get; set; }
        public string OwnerName { get; set; }
        public string OwnerMobileNo { get; set; }
        public string OccupierName { get; set; }
        public string OccupierMobileNo { get; set; }
        public string PropertyAddress { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Status { get; set; }
        public string ResurveyStatus { get; set; }
        public bool IsRead { get; set; }
        public string LastStep { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

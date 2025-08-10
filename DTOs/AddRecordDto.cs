namespace VirocGanpati.DTOs
{
    public class AddRecordDto
    {
        public string TenementNo { get; set; }
        public int WardId { get; set; }
        public string OldTenementNo { get; set; }
        public string OwnerName { get; set; }
        public string OwnerMobileNo { get; set; }
        public string OccupierName { get; set; }
        public string OccupierMobileNo { get; set; }
        public string PropertyAddress { get; set; }
        public string Status { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}

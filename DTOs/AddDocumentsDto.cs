namespace VirocGanpati.DTOs
{
    public class AddDocumentsDto
    {
        public int RecordId { get; set; }
        public List<DocumentUploadDto>? Documents { get; set; }
        public SignatureUploadDto? Signature { get; set; }
    }

    public class DocumentUploadDto
    {
        public IFormFile DocumentFile { get; set; }
        public string? Description { get; set; }
    }

    public class SignatureUploadDto
    {
        public IFormFile? SignatureFile { get; set; }
        public string? Description { get; set; }
    }
}

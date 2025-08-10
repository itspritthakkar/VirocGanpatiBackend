using VirocGanpati.DTOs;

public interface IDocumentService
{
    Task<List<DocumentDto>> AddMultipleDocumentsWithSignatureAsync(AddDocumentsDto documentDto);
    Task<DocumentDto> GetDocumentByIdAsync(int documentId);
    Task<List<DocumentDto>> GetDocumentsByRecordIdAsync(int recordId);

    Task DeleteDocumentByIdAsync(int documentId);

    //Task<int> GetTotalNonMigratedCountAsync();
    //Task MigrateDocumentsAsync(int batchSize, CancellationToken cancellationToken);
    //Task<string> CompressAndUploadImageAsync(int documentId);

    //Task CompressDocumentsAsync(int batchSize, CancellationToken cancellationToken);
}

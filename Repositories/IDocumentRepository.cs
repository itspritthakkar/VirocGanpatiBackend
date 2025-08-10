using VirocGanpati.Models;

public interface IDocumentRepository
{
    Task<Document> AddDocumentWithSignatureAsync(Document document);
    Task AddMultipleDocumentsAsync(List<Document> documents);
    Task<Document> GetDocumentByIdAsync(int documentId);
    Task<List<Document>> GetDocumentsByRecordIdAsync(int recordId);
    Task<Document> UpdateDocumentAsync(Document document);
    Task BulkUpdateDocumentsAsync(List<Document> documents);
    Task<Document> GetSignatureByRecordIdAsync(int recordId);
    Task DeleteDocumentByIdAsync(int documentId);
}

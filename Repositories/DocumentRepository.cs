using Microsoft.EntityFrameworkCore;
using VirocGanpati.Data;
using VirocGanpati.Models;

public class DocumentRepository : IDocumentRepository
{
    private readonly ApplicationDbContext _context;

    public DocumentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Document> AddDocumentWithSignatureAsync(Document document)
    {
        if (document.DocumentType == "Signature")
        {
            var existingSignature = await _context.Documents.FirstOrDefaultAsync(d => d.RecordId == document.RecordId && d.DocumentType == "Signature");
            if (existingSignature != null)
            {
                _context.Documents.Remove(existingSignature);
            }
        }
        _context.Documents.Add(document);
        await _context.SaveChangesAsync();

        return document;
    }
    public async Task AddMultipleDocumentsAsync(List<Document> documents)
    {
        _context.Documents.AddRange(documents);
        await _context.SaveChangesAsync();
    }

    public async Task<Document> GetDocumentByIdAsync(int documentId)
    {
        return await _context.Documents.FindAsync(documentId);
    }

    public async Task<List<Document>> GetDocumentsByRecordIdAsync(int recordId)
    {
        return await _context.Documents.Where(d => d.RecordId == recordId).ToListAsync();
    }

    public async Task<Document> UpdateDocumentAsync(Document document)
    {
        document.UpdatedAt = DateTime.UtcNow;
        _context.Entry(document).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return document;
    }

    public async Task BulkUpdateDocumentsAsync(List<Document> documents)
    {
        _context.Documents.UpdateRange(documents);
        await _context.SaveChangesAsync();
    }

    public async Task<Document> GetSignatureByRecordIdAsync(int recordId)
    {
        return await _context.Documents.Where(d => d.RecordId == recordId && d.DocumentType == "Signature").FirstOrDefaultAsync();
    }

    public async Task DeleteDocumentByIdAsync(int documentId)
    {
        var document = await _context.Documents.FindAsync(documentId);

        if (document != null)
        {
            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirocGanpati.DTOs;
using VirocGanpati.Helpers;

[ApiController]
[Route("api/[controller]")]
public class DocumentController : ControllerBase
{
    private readonly IDocumentService _documentService;

    public DocumentController(IDocumentService documentService)
    {
        _documentService = documentService;
    }

    [HttpPost("UploadMultipleDocuments")]
    [Authorize]
    public async Task<IActionResult> UploadMultipleDocuments([FromForm] AddDocumentsDto documentDto)
    {
        try
        {
            var createdDocuments = await _documentService.AddMultipleDocumentsWithSignatureAsync(documentDto);
            return ResponseHelper.OkMessage("Documents and signature uploaded successfully.");
        }
        catch (Exception ex)
        {
            return ResponseHelper.BadRequestMessage(ex.Message);
        }
    }

    [HttpGet("{documentId}")]
    public async Task<ActionResult<DocumentDto>> GetDocumentById(int documentId)
    {
        try
        {
            var document = await _documentService.GetDocumentByIdAsync(documentId);
            return Ok(document);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
    }

    // Endpoint to get documents by RecordId
    [HttpGet("by-record/{recordId}")]
    public async Task<ActionResult<List<DocumentDto>>> GetDocumentsByRecordId(int recordId)
    {
        var documents = await _documentService.GetDocumentsByRecordIdAsync(recordId);
        return Ok(documents);
    }

    [HttpDelete("{documentId}")]
    [Authorize]
    public async Task<IActionResult> DeleteDocumentById(int documentId)
    {
        try
        {
            await _documentService.DeleteDocumentByIdAsync(documentId);
            return ResponseHelper.OkMessage("Document deleted successfully.");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
    }

    //[HttpPut("Compress/{documentId}")]
    //[Authorize]
    //public async Task<IActionResult> CompressAndUploadImageAsync(int documentId)
    //{
    //    try
    //    {
    //        string fileKey = await _documentService.CompressAndUploadImageAsync(documentId);
    //        return ResponseHelper.OkMessage($"Document compressed successfully. New key: {fileKey}");
    //    }
    //    catch (KeyNotFoundException ex)
    //    {
    //        return NotFound(new { Message = ex.Message });
    //    }
    //}
}

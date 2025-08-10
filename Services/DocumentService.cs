using AutoMapper;
using Microsoft.Extensions.Hosting;
using SixLabors.ImageSharp.Formats.Jpeg;
using VirocGanpati.DTOs;
using VirocGanpati.Models;
using VirocGanpati.Services;
using SixLabors.ImageSharp;
using System.Text;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;

public class DocumentService : IDocumentService
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IS3Service _s3Service;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _environment;

    public DocumentService(IDocumentRepository documentRepository, IS3Service s3Service, IMapper mapper, IWebHostEnvironment environment)
    {
        _documentRepository = documentRepository;
        _s3Service = s3Service;
        _mapper = mapper;
        _environment = environment;
    }

    private IImageEncoder GetEncoderFromExtension(string extension)
    {
        switch (extension)
        {
            case ".jpg":
            case ".jpeg":
                return new JpegEncoder { Quality = 75 };
            case ".png":
                return new PngEncoder
                {
                    CompressionLevel = PngCompressionLevel.Level6, // adjust as needed
                    ColorType = PngColorType.RgbWithAlpha
                };
            default:
                return new JpegEncoder { Quality = 75 };
        }
    }

    public async Task<List<DocumentDto>> AddMultipleDocumentsWithSignatureAsync(AddDocumentsDto documentDto)
    {
        var resultList = new List<Document>();

        if (documentDto.Signature?.SignatureFile != null)
        {
            var signatureFileName = $"{Guid.NewGuid()}_{documentDto.Signature.SignatureFile.FileName}";

            // Compress signature image and upload
            string signatureFileKey;
            string signatureFileExtension = Path.GetExtension(documentDto.Signature.SignatureFile.FileName).ToLowerInvariant();
            using (var originalStream = documentDto.Signature.SignatureFile.OpenReadStream())
            using (var image = await Image.LoadAsync(originalStream))
            using (var compressedStream = new MemoryStream())
            {
                var encoder = GetEncoderFromExtension(signatureFileExtension);
                await image.SaveAsync(compressedStream, encoder);
                compressedStream.Position = 0;

                signatureFileKey = await _s3Service.UploadCompressedFileAsync(compressedStream, signatureFileName, documentDto.Signature.SignatureFile.ContentType);
            }

            Document existingSignature = await _documentRepository.GetSignatureByRecordIdAsync(documentDto.RecordId);
            if (existingSignature != null)
            {
                await DeleteDocumentByIdAsync(existingSignature.DocumentId);
            }

            var signatureDocument = new Document
            {
                RecordId = documentDto.RecordId,
                DocumentType = "Signature",
                FileName = signatureFileName,
                Extension = signatureFileExtension,
                Description = documentDto.Signature.Description,
                CreatedAt = DateTime.UtcNow,
            };

            var createdSignature = await _documentRepository.AddDocumentWithSignatureAsync(signatureDocument);
            resultList.Add(createdSignature);
        }

        if (documentDto.Documents != null)
        {
            var documentList = new List<Document>();

            foreach (var doc in documentDto.Documents)
            {
                var documentFileName = $"{Guid.NewGuid()}_{doc.DocumentFile.FileName}";

                // Compress document image and upload
                string documentFileKey;
                string documentFileExtension = Path.GetExtension(doc.DocumentFile.FileName).ToLowerInvariant();
                using (var originalStream = doc.DocumentFile.OpenReadStream())
                using (var image = await Image.LoadAsync(originalStream))
                using (var compressedStream = new MemoryStream())
                {
                    var encoder = GetEncoderFromExtension(documentFileExtension);
                    await image.SaveAsync(compressedStream, encoder);
                    compressedStream.Position = 0;

                    documentFileKey = await _s3Service.UploadCompressedFileAsync(compressedStream, documentFileName, doc.DocumentFile.ContentType);
                }

                var document = new Document
                {
                    RecordId = documentDto.RecordId,
                    DocumentType = "Document",
                    FileName = documentFileName,
                    Extension = documentFileExtension,
                    Description = doc.Description,
                    CreatedAt = DateTime.UtcNow,
                };

                documentList.Add(document);
                resultList.Add(document);
            }

            await _documentRepository.AddMultipleDocumentsAsync(documentList);
        }

        List<DocumentDto> documentDtoList = _mapper.Map<List<DocumentDto>>(resultList);

        return documentDtoList;
    }

    public async Task<DocumentDto> GetDocumentByIdAsync(int documentId)
    {
        var document = await _documentRepository.GetDocumentByIdAsync(documentId);
        if (document == null)
        {
            throw new KeyNotFoundException($"Document with ID {documentId} not found.");
        }

        DocumentDto documentDto = _mapper.Map<DocumentDto>(document);

        documentDto.FileUrl = documentDto.IsCompressed ? _s3Service.GetCompressedPreSignedUrl(documentDto.CompressedFileKey!) : _s3Service.GetPreSignedUrl(documentDto.FileKey!);

        return _mapper.Map<DocumentDto>(document);
    }

    public async Task<List<DocumentDto>> GetDocumentsByRecordIdAsync(int recordId)
    {
        var documents = await _documentRepository.GetDocumentsByRecordIdAsync(recordId);

        List<DocumentDto> documentDtoList = _mapper.Map<List<DocumentDto>>(documents);

        foreach (DocumentDto documentDtoItem in documentDtoList)
        {
            documentDtoItem.FileUrl = documentDtoItem.IsCompressed ? _s3Service.GetCompressedPreSignedUrl(documentDtoItem.CompressedFileKey!) : _s3Service.GetPreSignedUrl(documentDtoItem.FileKey!);
        }

        return documentDtoList;
    }

    public async Task DeleteDocumentByIdAsync(int documentId)
    {
        var document = await _documentRepository.GetDocumentByIdAsync(documentId);
        if (document == null)
        {
            throw new KeyNotFoundException($"Document with ID {documentId} not found.");
        }

        // Remove from database
        await _documentRepository.DeleteDocumentByIdAsync(documentId);
    }

    //public async Task<int> GetTotalNonMigratedCountAsync()
    //{
    //    return await _documentRepository.GetNonMigratedCountAsync();
    //}

    //public async Task MigrateDocumentsAsync(int batchSize, CancellationToken cancellationToken)
    //{
    //    // _logger.LogInformation("Starting document migration...");

    //    List<Document> documents;

    //    do
    //    {
    //        // Check for cancellation before fetching documents
    //        cancellationToken.ThrowIfCancellationRequested();

    //        documents = await _documentRepository.GetNonMigratedDocumentsAsync(batchSize);

    //        if (!documents.Any())
    //        {
    //            // _logger.LogInformation("No more documents to migrate.");
    //            return;
    //        }

    //        var migratedDocuments = new List<Document>();

    //        foreach (var document in documents)
    //        {
    //            // Check for cancellation before processing each document
    //            cancellationToken.ThrowIfCancellationRequested();

    //            try
    //            {
    //                string folderName = document.DocumentType == "Signature" ? "signatures" : "documents";
    //                string localFilePath = Path.Combine(_environment.WebRootPath, "uploads", folderName, document.FileName);

    //                if (!File.Exists(localFilePath))
    //                {
    //                    // _logger.LogWarning($"File not found: {localFilePath}");
    //                    continue;
    //                }

    //                string fileKey;
    //                using (var stream = new FileStream(localFilePath, FileMode.Open, FileAccess.Read))
    //                {
    //                    fileKey = await _s3Service.UploadFileAsync(stream, $"{Guid.NewGuid()}{document.Extension}", "application/octet-stream");
    //                }

    //                document.FileKey = fileKey;
    //                document.IsMigrated = true;
    //                document.UpdatedAt = DateTime.UtcNow;
    //                migratedDocuments.Add(document);

    //                //File.Delete(localFilePath);
    //                //_logger.LogInformation($"Migrated: {document.FileName} -> {fileKey}");
    //            }
    //            catch (Exception ex)
    //            {
    //                // _logger.LogError($"Error migrating file {document.FileName}: {ex.Message}");
    //            }
    //        }

    //        // Check for cancellation before bulk updating
    //        cancellationToken.ThrowIfCancellationRequested();

    //        if (migratedDocuments.Any())
    //        {
    //            await _documentRepository.BulkUpdateDocumentsAsync(migratedDocuments);
    //        }

    //    } while (documents.Count == batchSize);

    //    // _logger.LogInformation("Document migration completed.");
    //}

    //public async Task<Document> CompressAndUploadImageAsync(Document document)
    //{
    //    // Step 1: Fetch document info
    //    if (document == null)
    //    {
    //        throw new KeyNotFoundException($"Document passed to compressed not found.");
    //    }

    //    // Step 2: Download original image from S3
    //    using var originalImageStream = await _s3Service.DownloadFileAsync(document.FileKey!);

    //    // Measure original file size
    //    long originalSizeBytes = originalImageStream.Length;
    //    double originalSizeMB = originalSizeBytes / (1024.0 * 1024.0);

    //    string fileExtension = Path.GetExtension(document.FileName);

    //    // Step 3: Compress the image
    //    using var compressedImageStream = new MemoryStream();
    //    using (var image = await Image.LoadAsync(originalImageStream))
    //    {
    //        // Print original dimensions
    //        Console.WriteLine($"Original Dimensions: {image.Width} x {image.Height}");
    //        Console.WriteLine($"Original Size: {originalSizeMB:F2} MB");

    //        var encoder = GetEncoderFromExtension(fileExtension);
    //        image.Save(compressedImageStream, encoder);

    //        // Optionally if you want to check compressed dimensions (they will usually be same)
    //        Console.WriteLine($"Compressed Dimensions: {image.Width} x {image.Height}");
    //    }

    //    // Measure compressed file size
    //    long compressedSizeBytes = compressedImageStream.Length;
    //    double compressedSizeMB = compressedSizeBytes / (1024.0 * 1024.0);

    //    Console.WriteLine($"Compressed Size: {compressedSizeMB:F2} MB");

    //    compressedImageStream.Position = 0; // Reset stream before upload

    //    // Step 4: Upload compressed image back to S3
    //    var compressedFileName = $"compressed_{Guid.NewGuid()}{fileExtension}";
    //    var compressedFileKey = await _s3Service.UploadCompressedFileAsync(
    //        compressedImageStream,
    //        compressedFileName,
    //        "image/jpeg"
    //    );

    //    document.IsCompressed = true;
    //    document.CompressedFileKey = compressedFileKey;

    //    return document;
    //}

    //public async Task<string> CompressAndUploadImageAsync(int documentId)
    //{
    //    // Step 1: Fetch document info
    //    var document = await _documentRepository.GetDocumentByIdAsync(documentId);

    //    Document compressedDocument = await CompressAndUploadImageAsync(document);

    //    return compressedDocument.CompressedFileKey!;
    //}

}

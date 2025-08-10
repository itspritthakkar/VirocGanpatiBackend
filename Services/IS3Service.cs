namespace VirocGanpati.Services
{
    public interface IS3Service
    {
        Task<string> UploadFileAsync(Stream fileStream, string originalFileName, string contentType);
        Task<string> UploadCompressedFileAsync(Stream fileStream, string originalFileName, string contentType);
        string GetPreSignedUrl(string fileKey, int expiryInMinutes = 60);
        string GetCompressedPreSignedUrl(string fileKey, int expiryInMinutes = 60);
        Task<string> UpdateFileAsync(Stream newFileStream, string oldFileKey, string originalFileName, string contentType);
        Task<bool> DeleteFileAsync(string fileName);
        Task<bool> DeleteCompressedFileAsync(string fileKey);
        Task<Stream> DownloadFileAsync(string fileKey);
    }
}

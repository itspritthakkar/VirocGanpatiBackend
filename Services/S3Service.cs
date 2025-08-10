using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using VirocGanpati.Models;

namespace VirocGanpati.Services
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly AWSOptions _awsOptions;

        public S3Service(IOptions<AWSOptions> awsOptions)
        {
            _awsOptions = awsOptions.Value;
            _s3Client = new AmazonS3Client(
                _awsOptions.AccessKey,
                _awsOptions.SecretKey,
                RegionEndpoint.GetBySystemName(_awsOptions.Region)
            );
        }

        /// <summary>
        /// Uploads a file to S3 with a unique key and returns the key.
        /// </summary>
        public async Task<string> UploadFileAsync(Stream fileStream, string originalFileName, string contentType)
        {
            return await UploadFileCoreAsync(fileStream, originalFileName, contentType, _awsOptions.S3BucketName); // Store only the unique key in the database
        }

        public async Task<string> UploadCompressedFileAsync(Stream fileStream, string originalFileName, string contentType)
        {
            return await UploadFileCoreAsync(fileStream, originalFileName, contentType, _awsOptions.S3CompressedBucketName); // Store only the unique key in the database
        }

        public async Task<string> UploadFileCoreAsync(Stream fileStream, string originalFileName, string contentType, string bucketName)
        {
            string fileExtension = Path.GetExtension(originalFileName);
            string uniqueFileName = $"{Guid.NewGuid()}{fileExtension}"; // Generates a unique name

            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = uniqueFileName,
                InputStream = fileStream,
                ContentType = contentType,
            };

            await _s3Client.PutObjectAsync(request);
            return uniqueFileName; // Store only the unique key in the database
        }

        /// <summary>
        /// Generates a pre-signed URL to access a file.
        /// </summary>
        public string GetPreSignedUrl(string fileKey, int expiryInMinutes = 60)
        {
            return GetPreSignedUrlCore(fileKey, _awsOptions.S3BucketName, expiryInMinutes);
        }

        public string GetCompressedPreSignedUrl(string fileKey, int expiryInMinutes = 60)
        {
            return GetPreSignedUrlCore(fileKey, _awsOptions.S3CompressedBucketName, expiryInMinutes);
        }

        public string GetPreSignedUrlCore(string fileKey, string bucketName, int expiryInMinutes = 60)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = fileKey,
                Expires = DateTime.UtcNow.AddMinutes(expiryInMinutes),
                Protocol = Protocol.HTTPS
            };

            return _s3Client.GetPreSignedURL(request);
        }

        /// <summary>
        /// Updates an existing file by deleting the old file and uploading a new one.
        /// </summary>
        public async Task<string> UpdateFileAsync(Stream newFileStream, string oldFileKey, string originalFileName, string contentType)
        {
            await DeleteFileAsync(oldFileKey); // Delete the old file
            return await UploadCompressedFileAsync(newFileStream, originalFileName, contentType); // Upload new file with a new unique key
        }

        /// <summary>
        /// Deletes a file from S3.
        /// </summary>
        public async Task<bool> DeleteFileAsync(string fileKey)
        {
            return await DeleteFileCoreAsync(fileKey, _awsOptions.S3BucketName);
        }

        public async Task<bool> DeleteCompressedFileAsync(string fileKey)
        {
            return await DeleteFileCoreAsync(fileKey, _awsOptions.S3CompressedBucketName);
        }

        public async Task<bool> DeleteFileCoreAsync(string fileKey, string bucketName)
        {
            try
            {
                var request = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = fileKey
                };

                var response = await _s3Client.DeleteObjectAsync(request);
                return response.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false; // File does not exist
            }
        }

        public async Task<Stream> DownloadFileAsync(string fileKey)
        {
            var response = await _s3Client.GetObjectAsync(_awsOptions.S3BucketName, fileKey);
            var memoryStream = new MemoryStream();
            await response.ResponseStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            return memoryStream;
        }

    }
}

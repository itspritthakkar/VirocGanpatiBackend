namespace VirocGanpati.Models
{
    public class AWSOptions
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string Region { get; set; }
        public string S3BucketName { get; set; }
        public string S3CompressedBucketName { get; set; }

        // Base URL for accessing S3 files
        public string BaseUrl => $"https://{S3BucketName}.s3.{Region}.amazonaws.com/";

        public string CompressedBaseUrl => $"https://{S3CompressedBucketName}.s3.{Region}.amazonaws.com/";
    }
}

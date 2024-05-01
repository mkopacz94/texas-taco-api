namespace TexasTaco.Products.Api.Exceptions
{
    public class AwsS3BucketFileUploadException(string fileName, string error)
        : Exception($"Uploading {fileName} file to AWS S3 bucket failed due to an error: {error}");
}

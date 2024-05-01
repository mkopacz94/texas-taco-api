namespace TexasTaco.Products.Api.Configuration
{
    public class AwsS3BucketClientOptions
    {
        public string? ApiBaseAddress { get; init; }
        public string? ApiKey { get; init; }
        public string? BucketName { get; init; }
        public string? BucketUrl { get; init; }
    }
}

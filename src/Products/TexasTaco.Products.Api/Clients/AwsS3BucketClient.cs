using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using TexasTaco.Products.Api.Configuration;
using TexasTaco.Products.Api.Exceptions;
using TexasTaco.Products.Api.Model;

namespace TexasTaco.Products.Api.Clients
{
    internal class AwsS3BucketClient(
        HttpClient _httpClient,
        IOptions<AwsS3BucketClientOptions> _options,
        ILogger<AwsS3BucketClient> _logger) : IAwsS3BucketClient
    {
        public async Task<UploadedPicture> PutPictureAsync(
            byte[] pictureFileBytes,
            string fileName,
            string contentType,
            CancellationToken cancellationToken)
        {
            try
            {
                string fileExtension = Path.GetExtension(fileName);
                string uploadedFileName = $"{Guid.NewGuid()}{fileExtension}";
                string requestUri = Path.Combine(_options.Value.BucketName!, uploadedFileName);

                var httpContent = new ByteArrayContent(pictureFileBytes);
                httpContent.Headers.Add(HeaderNames.ContentType, contentType);

                var response = await _httpClient
                    .PutAsync(requestUri, httpContent, cancellationToken);

                response.EnsureSuccessStatusCode();

                string uploadedPictureUrl = Path
                    .Combine(_options.Value.BucketUrl!, uploadedFileName);

                return new UploadedPicture(uploadedPictureUrl);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Uploading picture to AWS S3 bucket " +
                    "failed with an exception: {exception}", ex.Message);

                throw new AwsS3BucketFileUploadException(
                    fileName,
                    $"AWS S3 bucket client responded with {ex.StatusCode} " +
                        $"status code and error: {ex.Message}");
            }
        }
    }
}

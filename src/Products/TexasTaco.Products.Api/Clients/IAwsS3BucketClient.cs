using TexasTaco.Products.Api.Model;

namespace TexasTaco.Products.Api.Clients
{
    internal interface IAwsS3BucketClient
    {
        Task<UploadedPicture> PutPictureAsync(
            byte[] pictureFileBytes,
            string fileName,
            string contentType,
            CancellationToken cancellationToken);
    }
}

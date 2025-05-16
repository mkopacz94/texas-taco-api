using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TexasTaco.Products.Api.Clients;
using TexasTaco.Products.Api.Exceptions;
using TexasTaco.Products.Core.Entities;
using TexasTaco.Products.Core.Mapping;
using TexasTaco.Products.Core.Repositories;

namespace TexasTaco.Products.Api.Endpoints.Pictures
{
    internal class AddPicture : IEndpoint
    {
        private const int MaximumPictureFileSizeInKB = 2048;
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("pictures", async (
                IFormFile pictureFile,
                [FromServices] IAwsS3BucketClient s3BucketClient,
                [FromServices] IPicturesRepository picturesRepository,
                [FromServices] ILogger<AddPicture> logger,
                CancellationToken cancellationToken) =>
            {
                var pictureFileBytes = await FileToByteArrayAsync(
                    pictureFile, cancellationToken);

                int pictureSizeInKB = CalculateSizeInKB(pictureFileBytes);

                logger.LogInformation("Picture file has a size of {size} KB.", pictureSizeInKB);

                if (pictureSizeInKB > MaximumPictureFileSizeInKB)
                {
                    throw new PictureSizeTooBigException(
                        pictureSizeInKB,
                        MaximumPictureFileSizeInKB);
                }

                var uploadedPicture = await s3BucketClient.PutPictureAsync(
                    pictureFileBytes,
                    pictureFile.FileName,
                    pictureFile.ContentType,
                    cancellationToken);

                var picture = new Picture(uploadedPicture.Url);
                await picturesRepository.AddAsync(picture);

                return Results.Created(
                    uploadedPicture.Url, PictureMap.Map(picture));
            })
            .RequireAuthorization()
            .WithTags(Tags.Pictures)
            .HasApiVersion(new ApiVersion(1))
            .Produces(StatusCodes.Status201Created, typeof(Picture))
            .Produces(StatusCodes.Status401Unauthorized);
        }

        private static async Task<byte[]> FileToByteArrayAsync(
            IFormFile pictureFile,
            CancellationToken cancellationToken)
        {
            using var memoryStream = new MemoryStream();
            await pictureFile.CopyToAsync(memoryStream, cancellationToken);
            return memoryStream.ToArray();
        }

        private static int CalculateSizeInKB(byte[] bytes) => bytes.Length / 1000;
    }
}

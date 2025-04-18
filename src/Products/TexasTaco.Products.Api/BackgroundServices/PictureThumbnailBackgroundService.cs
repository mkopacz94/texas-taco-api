using TexasTaco.Products.Api.Clients;
using TexasTaco.Products.Core.Repositories;
using TexasTaco.Products.Core.Services;

namespace TexasTaco.Products.Api.BackgroundServices
{
    internal class PictureThumbnailBackgroundService(
        IServiceProvider _serviceProvider,
        ILogger<PictureThumbnailBackgroundService> _logger,
        IHttpClientFactory _httpClientFactory)
        : BackgroundService
    {
        private const int ThumbnailSizePx = 75;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();

                var picturesRepository = scope
                    .ServiceProvider
                    .GetRequiredService<IPicturesRepository>();

                var pictureProcessor = scope
                    .ServiceProvider
                    .GetRequiredService<IPictureProcessor>();

                var awsBucketClient = scope
                    .ServiceProvider
                    .GetRequiredService<IAwsS3BucketClient>();

                var picturesWithoutThumbnail = (await picturesRepository
                    .GetPicturesWithoutThumbnailAsync())
                    .ToList();

                _logger.LogInformation("Found {count} pictures " +
                    "without generated thumbnail.", picturesWithoutThumbnail.Count);

                var client = _httpClientFactory.CreateClient();

                foreach (var picture in picturesWithoutThumbnail)
                {
                    if (picture.Product is null)
                    {
                        _logger.LogInformation("Picture {pictureId} has not got product assigned. " +
                            "Creating thumbnail skipped.", picture.Id);

                        continue;
                    }

                    _logger.LogInformation("Creating " +
                        "thumbnail for picture {pictureId}.", picture.Id);

                    var productImageBytes = await client
                        .GetByteArrayAsync(picture.Url, stoppingToken);

                    var resizedPictureBytes = await pictureProcessor
                        .ResizeImageAsync(
                            productImageBytes,
                            ThumbnailSizePx,
                            ThumbnailSizePx);

                    _logger.LogInformation("Successfully downsized picture " +
                        "{pictureId} to thumbnail size.", picture.Id);

                    var uploadedThumbnail = await awsBucketClient
                        .PutPictureAsync(
                            resizedPictureBytes,
                            $"{picture.Product.Name}.jpg",
                            "image/jpeg",
                            stoppingToken);

                    _logger.LogInformation("Successfully uploaded thumbnail of picture " +
                        "{pictureId} to AWS S3 bucket.", picture.Id);

                    picture.ThumbnailUrl = uploadedThumbnail.Url;
                    await picturesRepository.UpdateAsync(picture);
                }

                await Task.Delay(60000, stoppingToken);
            }
        }
    }
}

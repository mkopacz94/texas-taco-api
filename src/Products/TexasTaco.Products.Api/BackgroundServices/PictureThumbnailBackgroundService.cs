using TexasTaco.Products.Core.Repositories;

namespace TexasTaco.Products.Api.BackgroundServices
{
    internal class PictureThumbnailBackgroundService(
        IServiceProvider _serviceProvider,
        ILogger _logger,
        IHttpClientFactory _httpClientFactory)
        : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();

                var picturesRepository = scope
                    .ServiceProvider
                    .GetRequiredService<IPicturesRepository>();

                var picturesWithoutThumbnail = await picturesRepository
                    .GetPicturesWithoutThumbnailAsync();

                var client = _httpClientFactory.CreateClient();

                foreach (var picture in picturesWithoutThumbnail)
                {
                    _logger.LogInformation("Creating " +
                        "thumbnail for picture {pictureId}.", picture.Id);


                }

                await Task.Delay(60000, stoppingToken);
            }
        }
    }
}

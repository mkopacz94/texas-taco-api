using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace TexasTaco.Products.Core.Services
{
    internal class PictureProcessor : IPictureProcessor
    {
        public async Task<byte[]> ResizeImageAsync(
            byte[] imageBytes,
            int width,
            int height)
        {
            using var inputStream = new MemoryStream(imageBytes);
            using var image = Image.Load(inputStream);

            image.Mutate(i => i.Resize(new ResizeOptions
            {
                Size = new Size(width, height),
                Mode = ResizeMode.Crop,
                Position = AnchorPositionMode.Center
            }));

            using var outputStream = new MemoryStream();
            await image.SaveAsJpegAsync(outputStream);
            return outputStream.ToArray();
        }
    }
}

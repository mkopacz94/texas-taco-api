namespace TexasTaco.Products.Core.Services
{
    public interface IPictureProcessor
    {
        Task<byte[]> ResizeImageAsync(byte[] imageBytes, int width, int height);
    }
}

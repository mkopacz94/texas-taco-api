namespace TexasTaco.Products.Core.DTO
{
    public sealed record ProductDto(
        Guid Id,
        string Name,
        string ShortDescription,
        bool Recommended,
        decimal Price,
        PictureDto? Picture,
        string? ThumbnailUrl,
        string Category);
}

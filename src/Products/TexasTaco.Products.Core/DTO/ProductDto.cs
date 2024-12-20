namespace TexasTaco.Products.Core.DTO
{
    public sealed record ProductDto(
        Guid Id,
        string Name,
        string ShortDescription,
        bool Recommended,
        decimal Price,
        string? PictureUrl,
        string Category);
}

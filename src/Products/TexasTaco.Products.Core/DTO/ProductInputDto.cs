namespace TexasTaco.Products.Core.DTO
{
    public record ProductInputDto(
        string Name,
        string ShortDescription,
        bool Recommended,
        decimal Price,
        string PictureId);
}

namespace TexasTaco.Products.Core.DTO
{
    public record CategoryWithProductsDto(
        Guid Id,
        string Name,
        List<ProductDto> Products);
}

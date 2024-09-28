namespace TexasTaco.Products.Core.DTO
{
    public record PrizeInputDto(
        string Name, 
        string RequiredPointsAmount, 
        string ProductId, 
        string PictureId);
}

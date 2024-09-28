namespace TexasTaco.Products.Core.DTO
{
    public record PrizeInputDto(
        string Name, 
        int RequiredPointsAmount, 
        string ProductId, 
        string PictureId);
}

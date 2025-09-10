using TexasTaco.Products.Core.DTO;
using TexasTaco.Products.Core.Entities;

namespace TexasTaco.Products.Core.Mapping
{
    public static class PrizeMap
    {
        public static PrizeDto Map(Prize prize)
        {
            return new(
                prize.Id.Value,
                prize.ProductId.Value,
                prize.Name,
                prize.RequiredPointsAmount,
                new PictureDto(
                    prize.Picture.Id.Value,
                    prize.Picture.Url),
                prize.Picture.ThumbnailUrl);
        }
    }
}

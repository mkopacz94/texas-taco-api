using TexasTaco.Products.Core.ValueObjects;

namespace TexasTaco.Products.Core.Entities
{
    public class Prize(string name, int requiredPointsAmount)
    {
        public PrizeId Id { get; } = new PrizeId(Guid.NewGuid());
        public string Name { get; private set; } = name;
        public int RequiredPointsAmount { get; private set; } = requiredPointsAmount;
        public ProductId ProductId { get; private set; } = null!;
        public Product Product { get; private set; } = null!;
        public PictureId PictureId { get; private set; } = null!;
        public Picture Picture { get; private set; } = null!;

        public Prize(
            string name, 
            int requiredPointsAmount, 
            ProductId productId,
            PictureId pictureId)
            : this(name, requiredPointsAmount)
        {
            ProductId = productId;
            PictureId = pictureId;
        }
    }
}

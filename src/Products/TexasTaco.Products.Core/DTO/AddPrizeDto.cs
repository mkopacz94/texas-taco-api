namespace TexasTaco.Products.Core.DTO
{
    public class AddPrizeDto
    {
        public string Name { get; }
        public int RequiredPointsAmount { get; }
        public string ProductId { get; }
        public string PictureId { get; }

        public AddPrizeDto(
            string name, 
            int requiredPointsAmount, 
            string productId,
            string pictureId)
        {
            Name = name;
            RequiredPointsAmount = requiredPointsAmount;
            ProductId = productId;
            PictureId = pictureId;
        }
    }
}

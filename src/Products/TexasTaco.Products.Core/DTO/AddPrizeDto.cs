namespace TexasTaco.Products.Core.DTO
{
    public class AddPrizeDto
    {
        public string Name { get; }
        public int RequiredPointsAmount { get; }
        public string ProductId { get; }

        public AddPrizeDto(string name, int requiredPointsAmount, string productId)
        {
            Name = name;
            RequiredPointsAmount = requiredPointsAmount;
            ProductId = productId;
        }
    }
}

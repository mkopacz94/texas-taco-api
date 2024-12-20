using TexasTaco.Products.Core.ValueObjects;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Products.Core.Entities
{
    public class Product(
        string name,
        string shortDescription,
        bool recommended,
        decimal price)
    {
        private readonly List<Prize> _prizes = [];

        public ProductId Id { get; } = new ProductId(Guid.NewGuid());
        public string Name { get; private set; } = name;
        public string ShortDescription { get; private set; } = shortDescription;
        public bool Recommended { get; private set; } = recommended;
        public decimal Price { get; private set; } = price;
        public PictureId PictureId { get; private set; } = null!;
        public Picture Picture { get; private set; } = null!;
        public CategoryId CategoryId { get; private set; } = null!;
        public Category Category { get; private set; } = null!;
        public IReadOnlyCollection<Prize> Prizes => _prizes;

        public bool PriceChanged { get; private set; }

        public Product(
            string name,
            string shortDescription,
            bool recommended,
            decimal price,
            PictureId pictureId,
            CategoryId categoryId)
            : this(name, shortDescription, recommended, price)
        {
            PictureId = pictureId;
            CategoryId = categoryId;
        }

        public void UpdateProduct(
            string name,
            string shortDescription,
            bool recommended,
            decimal price,
            PictureId pictureId,
            CategoryId categoryId)
        {
            PriceChanged = Price != price;

            Name = name;
            ShortDescription = shortDescription;
            Recommended = recommended;
            Price = price;
            PictureId = pictureId;
            CategoryId = categoryId;
        }
    }
}

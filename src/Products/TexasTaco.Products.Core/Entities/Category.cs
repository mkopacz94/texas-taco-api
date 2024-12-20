using TexasTaco.Products.Core.ValueObjects;

namespace TexasTaco.Products.Core.Entities
{
    public class Category(string name)
    {
        private readonly List<Product> _products = [];

        public CategoryId Id { get; } = new CategoryId(Guid.NewGuid());
        public string Name { get; private set; } = name;
        public IReadOnlyCollection<Product> Products => _products;
    }
}

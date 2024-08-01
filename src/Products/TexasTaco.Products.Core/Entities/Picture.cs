using TexasTaco.Products.Core.ValueObjects;

namespace TexasTaco.Products.Core.Entities
{
    public sealed class Picture(string? url)
    {
        public PictureId Id { get; } = new PictureId(Guid.NewGuid());
        public string? Url { get; private set; } = url;
        public Product? Product { get; private set; }
        public Prize? Prize { get; private set; }
    }
}

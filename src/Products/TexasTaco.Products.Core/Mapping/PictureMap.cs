using TexasTaco.Products.Core.DTO;
using TexasTaco.Products.Core.Entities;

namespace TexasTaco.Products.Core.Mapping
{
    public static class PictureMap
    {
        public static PictureDto Map(Picture picture)
        {
            return new(picture.Id.Value, picture.Url);
        }
    }
}

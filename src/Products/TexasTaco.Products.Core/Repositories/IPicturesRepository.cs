using TexasTaco.Products.Core.Entities;
using TexasTaco.Products.Core.ValueObjects;

namespace TexasTaco.Products.Core.Repositories
{
    public interface IPicturesRepository
    {
        Task AddAsync(Picture picture);
        Task<bool> AnyAsync(PictureId pictureId);
        Task UpdateAsync(Picture picture);
        Task<IEnumerable<Picture>> GetPicturesWithoutThumbnailAsync();
    }
}

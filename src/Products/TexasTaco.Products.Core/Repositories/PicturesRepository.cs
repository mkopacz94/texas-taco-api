using Microsoft.EntityFrameworkCore;
using TexasTaco.Products.Core.Data.EF;
using TexasTaco.Products.Core.Entities;
using TexasTaco.Products.Core.ValueObjects;

namespace TexasTaco.Products.Core.Repositories
{
    internal class PicturesRepository(ProductsDbContext _context) : IPicturesRepository
    {
        public async Task AddAsync(Picture picture)
        {
            await _context.AddAsync(picture);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AnyAsync(PictureId pictureId)
        {
            return await _context
                .Pictures
                .AnyAsync(p => p.Id == pictureId);
        }

        public async Task UpdateAsync(Picture picture)
        {
            _context.Update(picture);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Picture>> GetPicturesWithoutThumbnailAsync()
        {
            return await _context
                .Pictures
                .Where(p => string.IsNullOrWhiteSpace(p.ThumbnailUrl))
                .ToListAsync();
        }
    }
}

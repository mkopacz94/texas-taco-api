using TexasTaco.Products.Core.Data.EF;
using TexasTaco.Products.Core.Entities;

namespace TexasTaco.Products.Core.Repositories
{
    internal class PicturesRepository(ProductsDbContext _context) : IPicturesRepository
    {
        public async Task AddAsync(Picture picture)
        {
            await _context.AddAsync(picture);
            await _context.SaveChangesAsync();
        }
    }
}

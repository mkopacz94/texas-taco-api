using TexasTaco.Products.Core.Entities;

namespace TexasTaco.Products.Core.Repositories
{
    public interface IPicturesRepository
    {
        Task AddAsync(Picture picture);
    }
}

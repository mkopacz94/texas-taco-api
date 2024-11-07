using TexasTaco.Orders.Domain.Basket;

namespace TexasTaco.Orders.Application.Baskets
{
    public interface IBasketRepository
    {
        Task AddAsync(Basket basket);
    }
}

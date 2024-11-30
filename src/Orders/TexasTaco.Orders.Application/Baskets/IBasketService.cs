using TexasTaco.Orders.Domain.Basket;

namespace TexasTaco.Orders.Application.Baskets
{
    public interface IBasketService
    {
        Task<Basket> AddItemToBasket(Guid accountId, BasketItem item);
    }
}

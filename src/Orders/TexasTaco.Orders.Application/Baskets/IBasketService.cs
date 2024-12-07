using TexasTaco.Orders.Domain.Basket;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Application.Baskets
{
    public interface IBasketService
    {
        Task<Basket> AddItemToBasket(AccountId accountId, BasketItem item);
    }
}

using TexasTaco.Orders.Domain.Cart;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Application.Carts
{
    public interface ICartService
    {
        Task<Cart> AddItemToCart(AccountId accountId, CartProduct product);
    }
}

using TexasTaco.Orders.Domain.Cart;

namespace TexasTaco.Orders.Application.Carts
{
    public interface ICheckoutCartsRepository
    {
        Task<Domain.Cart.CheckoutCart?> GetAsync(CheckoutCartId id);
        Task AddAsync(Domain.Cart.CheckoutCart checkoutCart);
        Task UpdateAsync(Domain.Cart.CheckoutCart checkoutCart);
    }
}

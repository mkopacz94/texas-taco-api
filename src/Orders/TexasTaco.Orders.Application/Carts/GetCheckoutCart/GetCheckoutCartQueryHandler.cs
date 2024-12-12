using MediatR;
using TexasTaco.Orders.Application.Carts.DTO;
using TexasTaco.Orders.Application.Carts.Exceptions;
using TexasTaco.Orders.Application.Carts.Mapping;

namespace TexasTaco.Orders.Application.Carts.GetCheckoutCart
{
    internal sealed class GetCheckoutCartQueryHandler(
        ICheckoutCartsRepository checkoutCartsRepository)
        : IRequestHandler<GetCheckoutCartQuery, CheckoutCartDto>
    {
        private readonly ICheckoutCartsRepository _checkoutCartsRepository
            = checkoutCartsRepository;

        public async Task<CheckoutCartDto> Handle(
            GetCheckoutCartQuery request,
            CancellationToken cancellationToken)
        {
            var checkoutCart = await _checkoutCartsRepository
                .GetAsync(request.Id)
                ?? throw new CheckoutCartNotFoundException(request.Id);

            return CheckoutCartMap.Map(checkoutCart);
        }
    }
}

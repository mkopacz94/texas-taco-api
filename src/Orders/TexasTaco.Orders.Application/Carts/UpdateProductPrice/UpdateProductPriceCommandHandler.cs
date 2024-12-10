using MediatR;

namespace TexasTaco.Orders.Application.Carts.UpdateProductPrice
{
    internal sealed class UpdateProductPriceCommandHandler(
        ICartsRepository _cartsRepository)
        : IRequestHandler<UpdateProductPriceCommand>
    {
        public async Task Handle(
            UpdateProductPriceCommand request,
            CancellationToken cancellationToken)
        {
            var cartsWithProduct = await _cartsRepository
                .GetCartsWithProduct(request.ProductId);

            foreach (var cart in cartsWithProduct)
            {
                var productsToUpdate = cart
                    .Products
                    .Where(bi => bi.ProductId == request.ProductId)
                    .ToList();

                productsToUpdate.ForEach(p => p.UpdatePrice(request.NewPrice));

                await _cartsRepository.UpdateAsync(cart);
            }
        }
    }
}

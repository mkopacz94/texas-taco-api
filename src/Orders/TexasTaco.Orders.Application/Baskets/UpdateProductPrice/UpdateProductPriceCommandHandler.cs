using MediatR;

namespace TexasTaco.Orders.Application.Baskets.UpdateProductPrice
{
    internal sealed class UpdateProductPriceCommandHandler(
        IBasketsRepository _basketsRepository)
        : IRequestHandler<UpdateProductPriceCommand>
    {
        public async Task Handle(
            UpdateProductPriceCommand request,
            CancellationToken cancellationToken)
        {
            var basketsWithProduct = await _basketsRepository
                .GetBasketsWithProduct(request.ProductId);

            foreach (var basket in basketsWithProduct)
            {
                var productsToUpdate = basket
                    .Items
                    .Where(bi => bi.ProductId == request.ProductId)
                    .ToList();

                productsToUpdate.ForEach(p => p.UpdatePrice(request.NewPrice));

                await _basketsRepository.UpdateAsync(basket);
            }
        }
    }
}

using TexasTaco.Products.Core.Data.EF;
using TexasTaco.Products.Core.DTO;
using TexasTaco.Products.Core.Entities;
using TexasTaco.Products.Core.Exceptions;
using TexasTaco.Products.Core.Repositories;
using TexasTaco.Products.Core.ValueObjects;
using TexasTaco.Shared.EventBus.Products;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Products.Core.Services
{
    internal class ProductUpdateService(
        IUnitOfWork unitOfWork,
        IProductsRepository productsRepository,
        IProductPriceChangedOutboxMessagesRepository outboxRepository)
        : IProductUpdateService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IProductsRepository _productsRepository = productsRepository;
        private readonly IProductPriceChangedOutboxMessagesRepository _outboxRepository = outboxRepository;

        public async Task UpdateProductAsync(
            ProductId productId, ProductInputDto updateData)
        {
            var productToUpdate = await _productsRepository.GetAsync(productId)
                ?? throw new ProductNotFoundException(productId);

            productToUpdate.UpdateProduct(
                updateData.Name,
                updateData.ShortDescription,
                updateData.Recommended,
                updateData.Price,
                new PictureId(Guid.Parse(updateData.PictureId)),
                new CategoryId(Guid.Parse(updateData.CategoryId)));

            using var transaction = await _unitOfWork.BeginTransactionAsync();
            await _productsRepository.UpdateAsync(productToUpdate);

            if (productToUpdate.PriceChanged)
            {
                var outboxMessageBody = new ProductPriceChangedEventMessage(
                    Guid.NewGuid(),
                    productId,
                    productToUpdate.Price);

                var outboxMessage = new ProductPriceChangedOutboxMessage(outboxMessageBody);

                await _outboxRepository.AddAsync(outboxMessage);

            }

            await transaction.CommitAsync();
        }
    }
}

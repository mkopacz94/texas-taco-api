using FluentAssertions;
using TexasTaco.Products.Core.Entities;
using TexasTaco.Products.Core.ValueObjects;

namespace TexasTaco.Products.Core.Tests.Entities
{
    public class ProductTests
    {
        [Theory]
        [InlineData(12.99, 14.99, true)]
        [InlineData(12.99, 10.99, true)]
        [InlineData(12.99, 0, true)]
        [InlineData(0, 12.99, true)]
        [InlineData(12.99, 12.99, false)]
        public void PriceChanged_Should_ReturnTrue_OnlyIfPriceChanged(
            decimal oldPrice, decimal newPrice, bool expectedPriceChanged)
        {
            //Arrange
            var pictureId = new PictureId(Guid.NewGuid());
            var categoryId = new CategoryId(Guid.NewGuid());
            var product = new Product(
                "OldProduct",
                "",
                false,
                oldPrice,
                pictureId,
                categoryId);

            //Act
            product.UpdateProduct(
                "NewProduct",
                "",
                false,
                newPrice,
                pictureId,
                categoryId);

            //Assert
            product
                .PriceChanged
                .Should()
                .Be(expectedPriceChanged);
        }
    }
}

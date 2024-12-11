using FluentAssertions;
using TexasTaco.Orders.Domain.Cart;
using TexasTaco.Orders.Domain.Cart.Exceptions;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Domain.Tests.Cart
{
    public class CartProductTests
    {
        [Fact]
        public void OnCreating_Should_ThrowInvalidCartProductQuantityException_IfQuantityIsLessThan1()
        {
            //Arrange / Act
            Action createAction = () => _ = new CartProduct(
                new ProductId(Guid.NewGuid()),
                "Product",
                10,
                null,
                -1);

            //Assert
            createAction
                .Should()
                .Throw<InvalidCartProductQuantityException>();
        }

        [Fact]
        public void OnCreating_Should_ThrowProductAmountExceededException_IfExceededMaximumAmountOfProducts()
        {
            //Arrange / Act
            Action createAction = () => _ = new CartProduct(
                new ProductId(Guid.NewGuid()),
                "Product",
                10,
                null,
                CartProduct.MaximumAmountOfProduct + 1);

            //Assert
            createAction
                .Should()
                .Throw<ProductAmountExceededException>();
        }

        [Fact]
        public void ChangeQuantity_Should_ChangeQuantityOfProduct()
        {
            //Arrange
            var product = new CartProduct(
                new ProductId(Guid.NewGuid()),
                "Product",
                10,
                null,
                4);

            int newQuantity = 2;

            //Act
            product.ChangeQuantity(newQuantity);

            //Assert
            product
                .Quantity
                .Should()
                .Be(newQuantity);
        }

        [Fact]
        public void ChangeQuantity_Should_ThrowInvalidCartProductQuantityException_IfQuantityHasNegativeValue()
        {
            //Arrange
            var product = new CartProduct(
                new ProductId(Guid.NewGuid()),
                "Product",
                10,
                null,
                4);

            int newQuantity = -2;

            //Act
            Action changeQuantityAction = () => product
                .ChangeQuantity(newQuantity);

            //Assert
            changeQuantityAction
                .Should()
                .Throw<InvalidCartProductQuantityException>();
        }

        [Fact]
        public void ChangeQuantity_Should_ThrowProductAmountExceededException_IfExceededMaximumAmountOfProducts()
        {
            //Arrange
            var product = new CartProduct(
                new ProductId(Guid.NewGuid()),
                "Product",
                10,
                null,
                4);

            //Act
            Action changeQuantityAction = () => product
                .ChangeQuantity(CartProduct.MaximumAmountOfProduct + 1);

            //Assert
            changeQuantityAction
                .Should()
                .Throw<ProductAmountExceededException>();
        }

        [Fact]
        public void IncreaseQuantity_Should_AddNewQuantityToOldQuantityOfProduct()
        {
            //Arrange
            int oldQuantity = 1;

            var product = new CartProduct(
                new ProductId(Guid.NewGuid()),
                "Product",
                10,
                null,
                oldQuantity);

            int newQuantity = 2;

            //Act
            product.IncreaseQuantity(newQuantity);

            //Assert
            product
                .Quantity
                .Should()
                .Be(oldQuantity + newQuantity);
        }

        [Fact]
        public void IncreaseQuantity_Should_ThrowInvalidCartProductQuantityException_IfQuantityHasNegativeValue()
        {
            //Arrange
            var product = new CartProduct(
                new ProductId(Guid.NewGuid()),
                "Product",
                10,
                null,
                4);

            int newQuantity = -2;

            //Act
            Action increaseQuantityAction = () => product
                .IncreaseQuantity(newQuantity);

            //Assert
            increaseQuantityAction
                .Should()
                .Throw<InvalidCartProductQuantityException>();
        }

        [Fact]
        public void IncreaseQuantity_Should_ThrowProductAmountExceededException_IfExceededMaximumAmountOfProducts()
        {
            //Arrange
            var product = new CartProduct(
                new ProductId(Guid.NewGuid()),
                "Product",
                10,
                null,
                4);

            //Act
            Action increaseQuantityAction = () => product
                .IncreaseQuantity(CartProduct.MaximumAmountOfProduct - product.Quantity + 1);

            //Assert
            increaseQuantityAction
                .Should()
                .Throw<ProductAmountExceededException>();
        }
    }
}

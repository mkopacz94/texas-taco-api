using TexasTaco.Orders.Domain.Cart;
using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Orders.Infrastructure.Data.EF;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Api.Tests.Data
{
    internal static class OrdersDatabaseSeeder
    {
        public static void Seed(OrdersDbContext context)
        {
            SeedCustomers(context);
            SeedCarts(context);
            context.SaveChanges();
        }

        private static void SeedCustomers(OrdersDbContext context)
        {
            if (context.Customers.Any())
            {
                return;
            }

            var customer = new Customer(
                new AccountId(Guid.NewGuid()),
                new EmailAddress("test@email.com"));

            context.Customers.Add(customer);
            context.SaveChanges();
        }

        private static void SeedCarts(OrdersDbContext context)
        {
            if (context.Carts.Any())
            {
                return;
            }

            var customer = context
                .Customers
                .First();

            var cart = new Cart(customer.Id);

            cart.AddProduct(new(
                ProductId.New(),
                "Product 1",
                15.99m,
                "Test_picture_url",
                3));

            cart.AddProduct(new(
                ProductId.New(),
                "Product 2",
                18.99m,
                "Test_picture_url",
                1));

            context.Carts.Add(cart);
            context.SaveChanges();
        }
    }
}

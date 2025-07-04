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
    }
}

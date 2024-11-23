using Microsoft.EntityFrameworkCore;
using TexasTaco.Orders.Application.Customers;
using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Orders.Infrastructure.Data.EF;

namespace TexasTaco.Orders.Infrastructure.Data.Repositories
{
    internal class CustomerRepository(OrdersDbContext _context) : ICustomerRepository
    {
        public async Task<Customer?> GetByAccountIdAsync(Guid accountId)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(u => u.AccountId == accountId);
        }

        public async Task AddAsync(Customer customer)
        {
            await _context.AddAsync(customer);
            await _context.SaveChangesAsync();
        }
    }
}

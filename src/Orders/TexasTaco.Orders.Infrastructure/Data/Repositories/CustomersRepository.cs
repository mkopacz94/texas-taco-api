using Microsoft.EntityFrameworkCore;
using TexasTaco.Orders.Application.Customers;
using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Orders.Infrastructure.Data.EF;

namespace TexasTaco.Orders.Infrastructure.Data.Repositories
{
    internal class CustomersRepository(OrdersDbContext _context) : ICustomersRepository
    {
        public async Task<Customer?> GetByAccountIdAsync(Guid accountId)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(u => u.AccountId == accountId);
        }

        public async Task<Customer?> GetByIdAsync(CustomerId id)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task AddAsync(Customer customer)
        {
            await _context.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            _context.Update(customer);
            await _context.SaveChangesAsync();
        }
    }
}

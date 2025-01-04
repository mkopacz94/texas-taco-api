using Microsoft.EntityFrameworkCore;
using TexasTaco.Orders.Application.Customers;
using TexasTaco.Orders.Application.Customers.Exceptions;
using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Orders.Infrastructure.Data.EF;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Infrastructure.Data.Repositories
{
    internal class CustomersRepository(OrdersDbContext _context) : ICustomersRepository
    {
        public async Task<Customer?> GetByAccountIdAsync(AccountId accountId)
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

        public async Task DeleteByAccountIdAsync(AccountId id)
        {
            var accountToDelete = await _context
                .Customers
                .FirstOrDefaultAsync(c => c.AccountId == id)
                ?? throw new CustomerNotFoundException(id);

            _context.Remove(accountToDelete);
            await _context.SaveChangesAsync();
        }
    }
}

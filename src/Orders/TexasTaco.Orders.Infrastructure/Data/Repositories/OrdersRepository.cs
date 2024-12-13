using Microsoft.EntityFrameworkCore;
using TexasTaco.Orders.Application.Orders;
using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Orders.Domain.Orders;
using TexasTaco.Orders.Infrastructure.Data.EF;

namespace TexasTaco.Orders.Infrastructure.Data.Repositories
{
    internal class OrdersRepository(OrdersDbContext context) : IOrdersRepository
    {
        private readonly OrdersDbContext _context = context;

        public async Task<Order?> GetByCustomerIdAsync(CustomerId customerId)
        {
            return await _context
                .Orders
                .FirstOrDefaultAsync(o => o.CustomerId == customerId);
        }

        public async Task<Order?> GetAsync(OrderId id)
        {
            return await _context
                .Orders
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task AddAsync(Order order)
        {
            await _context.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}

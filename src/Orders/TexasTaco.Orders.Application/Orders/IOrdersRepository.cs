using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Orders.Domain.Orders;

namespace TexasTaco.Orders.Application.Orders
{
    public interface IOrdersRepository
    {
        Task<Order?> GetByCustomerIdAsync(CustomerId customerId);
        Task<Order?> GetAsync(OrderId id);
        Task AddAsync(Order order);
    }
}

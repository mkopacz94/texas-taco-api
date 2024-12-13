using TexasTaco.Orders.Domain.Orders;

namespace TexasTaco.Orders.Application.Orders
{
    public interface IOrdersRepository
    {
        Task<Order?> GetAsync(OrderId id);
        Task AddAsync(Order order);
    }
}

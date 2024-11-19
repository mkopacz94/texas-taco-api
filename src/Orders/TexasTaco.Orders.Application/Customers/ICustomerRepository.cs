using TexasTaco.Orders.Domain.Customers;

namespace TexasTaco.Orders.Application.Customers
{
    public interface ICustomerRepository
    {
        Task AddAsync(Customer customer);
    }
}

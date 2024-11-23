using TexasTaco.Orders.Domain.Customers;

namespace TexasTaco.Orders.Application.Customers
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByAccountIdAsync(Guid accountId);
        Task AddAsync(Customer customer);
    }
}

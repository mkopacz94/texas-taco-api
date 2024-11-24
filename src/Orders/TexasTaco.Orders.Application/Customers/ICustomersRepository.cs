using TexasTaco.Orders.Domain.Customers;

namespace TexasTaco.Orders.Application.Customers
{
    public interface ICustomersRepository
    {
        Task<Customer?> GetByAccountIdAsync(Guid accountId);
        Task<Customer?> GetByIdAsync(CustomerId id);
        Task AddAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
    }
}

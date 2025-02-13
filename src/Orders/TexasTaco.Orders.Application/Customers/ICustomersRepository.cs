using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Application.Customers
{
    public interface ICustomersRepository
    {
        Task<Customer?> GetByAccountIdAsync(AccountId accountId);
        Task<Customer?> GetByIdAsync(CustomerId id);
        Task AddAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteByAccountIdAsync(AccountId id);
    }
}

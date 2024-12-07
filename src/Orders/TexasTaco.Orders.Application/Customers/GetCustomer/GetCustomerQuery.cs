using MediatR;
using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Application.Customers.GetCustomer
{
    public record GetCustomerQuery(AccountId AccountId) : IRequest<Customer>;
}

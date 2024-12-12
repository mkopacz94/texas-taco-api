using MediatR;
using TexasTaco.Orders.Application.Customers.DTO;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Application.Customers.GetCustomer
{
    public record GetCustomerQuery(AccountId AccountId)
        : IRequest<CustomerDto>;
}

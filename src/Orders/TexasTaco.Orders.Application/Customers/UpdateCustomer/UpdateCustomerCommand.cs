using MediatR;
using TexasTaco.Orders.Domain.Customers;

namespace TexasTaco.Orders.Application.Customers.UpdateCustomer
{
    public sealed record UpdateCustomerCommand(
        CustomerId CustomerId,
        string FirstName,
        string LastName,
        Address Address) : IRequest;
}

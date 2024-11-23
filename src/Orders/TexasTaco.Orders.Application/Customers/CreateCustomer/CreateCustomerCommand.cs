using MediatR;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Application.Customers.CreateCustomer
{
    public sealed record CreateCustomerCommand(
        Guid AccountId,
        EmailAddress EmailAddress)
        : IRequest;
}

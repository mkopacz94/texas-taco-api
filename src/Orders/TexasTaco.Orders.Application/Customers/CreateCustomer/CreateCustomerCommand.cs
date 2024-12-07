using MediatR;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Application.Customers.CreateCustomer
{
    public sealed record CreateCustomerCommand(
        AccountId AccountId,
        EmailAddress EmailAddress)
        : IRequest;
}

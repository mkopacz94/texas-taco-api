using MediatR;
using TexasTaco.Orders.Application.Customers.Exceptions;
using TexasTaco.Orders.Domain.Customers;

namespace TexasTaco.Orders.Application.Customers.CreateCustomer
{
    internal class CreateCustomerCommandHandler(
        ICustomersRepository _customersRepository)
        : IRequestHandler<CreateCustomerCommand>
    {
        public async Task Handle(
            CreateCustomerCommand request,
            CancellationToken cancellationToken)
        {
            var customerWithSameAccount = await _customersRepository
                .GetByAccountIdAsync(request.AccountId);

            if (customerWithSameAccount is not null)
            {
                throw new CustomerWithAccoundIdAlreadyExistsException(request.AccountId);
            }

            var customer = new Customer(request.AccountId, request.EmailAddress);

            await _customersRepository.AddAsync(customer);
        }
    }
}

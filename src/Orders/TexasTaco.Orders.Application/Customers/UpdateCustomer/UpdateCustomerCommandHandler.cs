using MediatR;
using TexasTaco.Orders.Application.Customers.Exceptions;

namespace TexasTaco.Orders.Application.Customers.UpdateCustomer
{
    internal class UpdateCustomerCommandHandler(
        ICustomersRepository _customersRepository)
        : IRequestHandler<UpdateCustomerCommand>
    {
        public async Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customerToUpdate = await _customersRepository
                .GetByIdAsync(request.CustomerId)
                ?? throw new CustomerNotFoundException(request.CustomerId);

            customerToUpdate.UpdateCustomer(
                request.FirstName, request.LastName, request.Address);

            await _customersRepository.UpdateCustomerAsync(customerToUpdate);
        }
    }
}

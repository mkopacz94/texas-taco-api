using MediatR;
using TexasTaco.Orders.Application.Customers.DTO;
using TexasTaco.Orders.Application.Customers.Exceptions;
using TexasTaco.Orders.Application.Customers.Mapping;

namespace TexasTaco.Orders.Application.Customers.GetCustomer
{
    internal class GetCustomerQueryHandler(ICustomersRepository customersRepository)
        : IRequestHandler<GetCustomerQuery, CustomerDto>
    {
        private readonly ICustomersRepository _customersRepository = customersRepository;

        public async Task<CustomerDto> Handle(
            GetCustomerQuery request,
            CancellationToken cancellationToken)
        {
            var customer = await _customersRepository
                .GetByAccountIdAsync(request.AccountId)
                ?? throw new CustomerNotFoundException(request.AccountId);

            return CustomerMap.Map(customer);
        }
    }
}

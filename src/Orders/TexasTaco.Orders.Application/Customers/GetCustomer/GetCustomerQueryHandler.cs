using MediatR;
using TexasTaco.Orders.Application.Customers.Exceptions;
using TexasTaco.Orders.Domain.Customers;

namespace TexasTaco.Orders.Application.Customers.GetCustomer
{
    internal class GetCustomerQueryHandler(ICustomersRepository customersRepository)
        : IRequestHandler<GetCustomerQuery, Customer>
    {
        private readonly ICustomersRepository _customersRepository = customersRepository;

        public async Task<Customer> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            return await _customersRepository
                .GetByAccountIdAsync(request.AccountId)
                ?? throw new CustomerNotFoundException(request.AccountId);
        }
    }
}

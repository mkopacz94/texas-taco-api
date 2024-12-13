using MediatR;
using TexasTaco.Orders.Application.Orders.DTO;
using TexasTaco.Orders.Application.Orders.Exceptions;
using TexasTaco.Orders.Application.Orders.Mapping;

namespace TexasTaco.Orders.Application.Orders.GetCustomerOrder
{
    internal class GetCustomerOrderQueryHandler(
        IOrdersRepository ordersRepository)
        : IRequestHandler<GetCustomerOrderQuery, OrderDto>
    {
        private readonly IOrdersRepository _ordersRepository = ordersRepository;

        public async Task<OrderDto> Handle(
            GetCustomerOrderQuery request,
            CancellationToken cancellationToken)
        {
            var order = await _ordersRepository
                .GetByCustomerIdAsync(request.CustomerId)
                ?? throw new OrderNotFoundException(request.CustomerId);

            return OrderMap.Map(order);
        }
    }
}

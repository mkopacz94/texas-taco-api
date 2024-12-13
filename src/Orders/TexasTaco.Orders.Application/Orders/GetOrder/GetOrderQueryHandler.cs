using MediatR;
using TexasTaco.Orders.Application.Orders.DTO;
using TexasTaco.Orders.Application.Orders.Exceptions;
using TexasTaco.Orders.Application.Orders.Mapping;

namespace TexasTaco.Orders.Application.Orders.GetOrder
{
    internal class GetOrderQueryHandler(
        IOrdersRepository ordersRepository)
        : IRequestHandler<GetOrderQuery, OrderDto>
    {
        private readonly IOrdersRepository _ordersRepository = ordersRepository;

        public async Task<OrderDto> Handle(
            GetOrderQuery request,
            CancellationToken cancellationToken)
        {
            var order = await _ordersRepository
                .GetAsync(request.Id)
                ?? throw new OrderNotFoundException(request.Id);

            return OrderMap.Map(order);
        }
    }
}

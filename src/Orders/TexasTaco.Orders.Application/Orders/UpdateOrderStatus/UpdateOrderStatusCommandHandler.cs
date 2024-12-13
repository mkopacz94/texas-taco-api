using MediatR;
using TexasTaco.Orders.Application.Orders.Exceptions;

namespace TexasTaco.Orders.Application.Orders.UpdateOrderStatus
{
    internal class UpdateOrderStatusCommandHandler(
        IOrdersRepository ordersRepository)
        : IRequestHandler<UpdateOrderStatusCommand>
    {
        private readonly IOrdersRepository _ordersRepository
            = ordersRepository;

        public async Task Handle(
            UpdateOrderStatusCommand request,
            CancellationToken cancellationToken)
        {
            var order = await _ordersRepository
                .GetAsync(request.Id)
                ?? throw new OrderNotFoundException(request.Id);

            order.UpdateStatus(request.Status);
            await _ordersRepository.UpdateAsync(order);
        }
    }
}

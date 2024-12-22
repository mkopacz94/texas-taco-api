using MediatR;
using TexasTaco.Orders.Application.Carts.Exceptions;
using TexasTaco.Orders.Application.Customers;
using TexasTaco.Orders.Application.Customers.Exceptions;
using TexasTaco.Orders.Application.Orders;
using TexasTaco.Orders.Application.Orders.DTO;
using TexasTaco.Orders.Application.Orders.Mapping;
using TexasTaco.Orders.Application.Shared;
using TexasTaco.Shared.EventBus.Orders;
using TexasTaco.Shared.Outbox;
using TexasTaco.Shared.Outbox.Repository;

namespace TexasTaco.Orders.Application.Carts.PlaceOrder
{
    internal class PlaceOrderCommandHandler(
        IUnitOfWork unitOfWork,
        ICartsRepository cartsRepository,
        ICheckoutCartsRepository checkoutCartsRepository,
        IOrdersRepository ordersRepository,
        ICustomersRepository customersRepository,
        IOutboxMessagesRepository<OutboxMessage<PointsCollectedEventMessage>>
            pointsCollectedOutboxRepository)
        : IRequestHandler<PlaceOrderCommand, OrderDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICartsRepository _cartsRepository = cartsRepository;
        private readonly ICheckoutCartsRepository _checkoutCartsRepository
            = checkoutCartsRepository;
        private readonly IOrdersRepository _ordersRepository
            = ordersRepository;
        private readonly ICustomersRepository _customersRepository
            = customersRepository;
        private readonly IOutboxMessagesRepository<OutboxMessage<PointsCollectedEventMessage>>
            _pointsCollectedOutboxRepository = pointsCollectedOutboxRepository;

        public async Task<OrderDto> Handle(
            PlaceOrderCommand request,
            CancellationToken cancellationToken)
        {
            var checkoutCart = await _checkoutCartsRepository
                .GetAsync(request.CheckoutCartId)
                ?? throw new CheckoutCartNotFoundException(request.CheckoutCartId);

            var order = checkoutCart.PlaceOrder();

            var customer = await _customersRepository
                .GetByIdAsync(order.CustomerId)
                ?? throw new CustomerNotFoundException(order.CustomerId);

            customer.AddPoints(order);

            await _unitOfWork.ExecuteTransactionAsync(async () =>
            {
                await _ordersRepository.AddAsync(order);
                await _customersRepository.UpdateCustomerAsync(customer);
                await _cartsRepository.DeleteAsync(checkoutCart.CartId);

                var message = new PointsCollectedEventMessage(
                    Guid.NewGuid(),
                    customer.AccountId.Value,
                    order.PointsCollected);

                var pointsCollectedMessage = new OutboxMessage<PointsCollectedEventMessage>(
                    message);

                await _pointsCollectedOutboxRepository
                    .AddAsync(pointsCollectedMessage);

            }, cancellationToken);

            return OrderMap.Map(order);
        }
    }
}

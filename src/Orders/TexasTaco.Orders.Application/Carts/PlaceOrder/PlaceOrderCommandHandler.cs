using MediatR;
using TexasTaco.Orders.Application.Carts.Exceptions;
using TexasTaco.Orders.Application.Customers;
using TexasTaco.Orders.Application.Customers.Exceptions;
using TexasTaco.Orders.Application.Orders;
using TexasTaco.Orders.Application.Orders.DTO;
using TexasTaco.Orders.Application.Orders.Mapping;
using TexasTaco.Orders.Application.Shared;

namespace TexasTaco.Orders.Application.Carts.PlaceOrder
{
    internal class PlaceOrderCommandHandler(
        IUnitOfWork unitOfWork,
        ICartsRepository cartsRepository,
        ICheckoutCartsRepository checkoutCartsRepository,
        IOrdersRepository ordersRepository,
        ICustomersRepository customersRepository)
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

            customer.AddPoints(order.CalculatePoints());

            await _unitOfWork.ExecuteTransactionAsync(async () =>
            {
                await _ordersRepository.AddAsync(order);
                await _customersRepository.UpdateCustomerAsync(customer);
                await _cartsRepository.DeleteAsync(checkoutCart.CartId);
            }, cancellationToken);

            return OrderMap.Map(order);
        }
    }
}

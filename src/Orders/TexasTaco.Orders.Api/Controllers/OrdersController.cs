using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TexasTaco.Orders.Application.Orders.GetCustomerOrder;
using TexasTaco.Orders.Application.Orders.GetOrder;
using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Orders.Domain.Orders;
using TexasTaco.Shared.Exceptions;

namespace TexasTaco.Orders.Api.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/[controller]")]
    [Authorize]
    public class OrdersController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetCustomerOrder([FromQuery] string customerId)
        {
            if (!Guid.TryParse(customerId, out var customerIdGuid))
            {
                throw new InvalidRequestParametersException(
                    $"Given customer ID ({customerId}) is not a valid GUID.");
            }

            var customerIdentifier = new CustomerId(Guid.Parse(customerId));
            var query = new GetCustomerOrderQuery(customerIdentifier);
            var orderDto = await _mediator.Send(query);

            return Ok(orderDto);
        }

        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<IActionResult> GetOrder(string id)
        {
            if (!Guid.TryParse(id, out var idGuid))
            {
                throw new InvalidRequestParametersException(
                    $"Given order ID ({id}) is not a valid GUID.");
            }

            var orderId = new OrderId(Guid.Parse(id));
            var query = new GetOrderQuery(orderId);
            var orderDto = await _mediator.Send(query);

            return Ok(orderDto);
        }
    }
}

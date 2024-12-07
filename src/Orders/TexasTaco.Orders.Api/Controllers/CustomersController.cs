using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TexasTaco.Orders.Application.Customers.GetCustomer;
using TexasTaco.Shared.Exceptions;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Api.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/orders/[controller]")]
    [Authorize]
    public sealed class CustomersController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetCustomer([FromQuery] string accountId)
        {
            if (!Guid.TryParse(accountId, out var accountIdGuid))
            {
                throw new InvalidRequestParametersException(
                    $"Given accountId ({accountId}) is not a valid GUID.");
            }

            var accountIdentifier = new AccountId(accountIdGuid);
            var query = new GetCustomerQuery(accountIdentifier);
            var customer = await _mediator.Send(query);

            return Ok(customer);
        }
    }
}

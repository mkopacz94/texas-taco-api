using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TexasTaco.Orders.Application.Baskets;
using TexasTaco.Orders.Domain.Basket;
using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Api.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/orders/[controller]")]
    [Authorize]
    public class BasketController(IBasketRepository _basketRepository) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddBasket()
        {
            var basketItem1 = new BasketItem(new ProductId(Guid.NewGuid()), "Tacos 3", 10.99m, "picture_url_1", 1);
            var basketItem2 = new BasketItem(new ProductId(Guid.NewGuid()), "Tacos 5", 5.43m, "picture_url_2", 3);

            var basket = new Basket(new CustomerId(Guid.NewGuid()));
            basket.AddProduct(basketItem1);
            basket.AddProduct(basketItem2);

            await _basketRepository.AddAsync(basket);

            return Ok(basket);
        }
    }
}

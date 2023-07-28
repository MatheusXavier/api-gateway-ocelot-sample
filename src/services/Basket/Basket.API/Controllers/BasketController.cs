using Basket.API.Infrastructure.Repositories;
using Basket.API.Model;

using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

[ApiController]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _repository;

    public BasketController(IBasketRepository repository)
    {
        _repository = repository;
    }

    [HttpDelete("api/v1/customers/{id:guid}/baskets/{productId:int}")]
    public async Task<IActionResult> RemoveBasketItem(Guid id, int productId)
    {
        CustomerBasket? basket = await _repository.GetBasketAsync(id);

        if (basket is null)
        {
            return Ok();
        }

        basket.Items.RemoveAll(p => p.Id == productId);

        await _repository.UpdateBasketAsync(basket);

        return Ok();
    }
}
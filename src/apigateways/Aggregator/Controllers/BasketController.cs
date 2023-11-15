using Aggregator.Models;
using Aggregator.Services;

using Microsoft.AspNetCore.Mvc;

namespace Aggregator.Controllers;

[ApiController]
public class BasketController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IBasketService _basketService;

    public BasketController(
        IProductService productService,
        IBasketService basketService)
    {
        _productService = productService;
        _basketService = basketService;
    }

    [HttpGet("api/v1/customers/{id:guid}/baskets")]
    public async Task<IActionResult> GetBasketByIdAsync(Guid id)
    {
        CustomerBasket basket = new(id, new List<BasketItem>());

        List<BasketItemData> basketItems = await _basketService.GetBasketItemsAsync(id);

        if (!basketItems.Any())
        {
            return Ok(basket); // Return empty basket
        }

        List<int> productIds = basketItems.Select(b => b.ProductId).ToList();

        List<ProductSummary> products = await _productService.GetProductByIdsAsync(productIds);

        foreach (BasketItemData item in basketItems)
        {
            ProductSummary? product = products.FirstOrDefault(p => p.Id == item.ProductId);

            if (product is null) // Could not found product, so we are not going to return it
            {
                continue;
            }

            basket.Items.Add(new BasketItem(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                item.Quantity));
        }

        return Ok(basket ?? new CustomerBasket(id, new List<BasketItem>()));
    }

    [HttpPost("api/v1/customers/{id:guid}/baskets")]
    public async Task<IActionResult> AddBasketItemAsync(Guid id, BasketItemData basketItem)
    {
        if (basketItem.Quantity <= 0)
        {
            return BadRequest("Invalid quantity");
        }

        bool productExists = await _productService.DoesProductExistAsync(basketItem.ProductId);

        if (!productExists)
        {
            return BadRequest("Invalid product id");
        }

        await _basketService.AddProductToBasketAsync(id, basketItem.ProductId, basketItem.Quantity);

        return Ok();
    }
}
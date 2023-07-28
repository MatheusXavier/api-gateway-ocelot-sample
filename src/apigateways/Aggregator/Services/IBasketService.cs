using Aggregator.Models;

namespace Aggregator.Services;

public interface IBasketService
{
    Task AddProductToBasketAsync(Guid customerId, int productId, int quantity);

    Task<List<BasketItemData>> GetBasketItemsAsync(Guid customerId);
}

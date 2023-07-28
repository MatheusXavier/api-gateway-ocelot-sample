using Aggregator.Models;

using BasketApi;

namespace Aggregator.Services;

public class BasketService : IBasketService
{
    private readonly BasketGrpc.BasketGrpcClient _client;

    public BasketService(BasketGrpc.BasketGrpcClient client)
    {
        _client = client;
    }

    public async Task AddProductToBasketAsync(Guid customerId, int productId, int quantity)
    {
        AddBasketItemRequest request = new()
        {
            CustomerId = customerId.ToString(),
            ProductId = productId,
            Quantity = quantity,
        };

        await _client.AddBasketItemAsync(request);
    }

    public async Task<List<BasketItemData>> GetBasketItemsAsync(Guid customerId)
    {
        GetBasketRequest request = new()
        {
            CustomerId = customerId.ToString(),
        };

        GetBasketResponse response = await _client.GetBasketAsync(request);

        return response.Items
            .Select(i => new BasketItemData(i.ProductId, i.Quantity))
            .ToList();
    }
}
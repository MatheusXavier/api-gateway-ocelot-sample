using Aggregator.Models;

using ProductApi;

namespace Aggregator.Services;

public class ProductService : IProductService
{
    private readonly ProductGrpc.ProductGrpcClient _client;

    public ProductService(ProductGrpc.ProductGrpcClient client)
    {
        _client = client;
    }

    public async Task<bool> DoesProductExistAsync(int productId)
    {
        DoesProductExistRequest request = new()
        {
            Id = productId
        };

        DoesProductExistResponse response = await _client.DoesProductExistAsync(request);

        return response.ProductExists;
    }

    public async Task<List<ProductSummary>> GetProductByIdsAsync(List<int> productIds)
    {
        GetProductByIdsRequest request = new();

        request.Ids.AddRange(productIds);

        GetProductByIdsResponse response = await _client.GetProductByIdsAsync(request);

        return response.Products
            .Select(p => new ProductSummary(p.Id, p.Name, p.Description, p.Price))
            .ToList();
    }
}
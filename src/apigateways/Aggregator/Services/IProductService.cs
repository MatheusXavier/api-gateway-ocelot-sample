using Aggregator.Models;

namespace Aggregator.Services;

public interface IProductService
{
    Task<bool> DoesProductExistAsync(int productId);

    Task<List<ProductSummary>> GetProductByIdsAsync(List<int> productIds);
}

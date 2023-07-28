using Grpc.Core;

using Microsoft.EntityFrameworkCore;

using Product.API.Infrastructure;
using Product.API.Model;

using ProductApi;

namespace Product.API.Services;

public class ProductService : ProductGrpc.ProductGrpcBase
{
    private readonly ProductContext _context;

    public ProductService(ProductContext context)
    {
        _context = context;
    }

    public override async Task<DoesProductExistResponse> DoesProductExist(
        DoesProductExistRequest request,
        ServerCallContext context)
    {
        bool productExists = await _context.Products
            .Where(p => p.Id == request.Id)
            .AsNoTracking()
            .AnyAsync();

        return new DoesProductExistResponse
        {
            ProductExists = productExists,
        };
    }

    public override async Task<GetProductByIdsResponse> GetProductByIds(
        GetProductByIdsRequest request,
        ServerCallContext context)
    {
        List<ProductItem> products = await _context.Products
            .Where(p => request.Ids.Contains(p.Id))
            .AsNoTracking()
            .ToListAsync();

        GetProductByIdsResponse response = new();

        foreach (ProductItem product in products)
        {
            response.Products.Add(new ProductSummaryResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            });
        }

        return response;
    }
}

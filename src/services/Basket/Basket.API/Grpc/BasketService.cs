using Basket.API.Infrastructure.Repositories;
using Basket.API.Model;

using BasketApi;

using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

namespace Basket.API.Grpc;

public class BasketService : BasketGrpc.BasketGrpcBase
{
    private readonly IBasketRepository _repository;

    public BasketService(IBasketRepository repository)
    {
        _repository = repository;
    }

    public override async Task<GetBasketResponse> GetBasket(GetBasketRequest request, ServerCallContext context)
    {
        Guid customerId = Guid.Parse(request.CustomerId);
        CustomerBasket? basket = await _repository.GetBasketAsync(customerId);

        GetBasketResponse response = new();

        if (basket is not null)
        {
            response.Items.AddRange(basket.Items.Select(i => new BasketItemResponse
            {
                ProductId = i.Id,
                Quantity = i.Quantity,
            }));
        }

        return response;
    }

    public override async Task<Empty> AddBasketItem(AddBasketItemRequest request, ServerCallContext context)
    {
        Guid customerId = Guid.Parse(request.CustomerId);
        CustomerBasket? basket = await _repository.GetBasketAsync(customerId);

        basket ??= new CustomerBasket(customerId, Items: new List<BasketItem>());

        BasketItem? product = basket.Items.FirstOrDefault(p => p.Id == request.ProductId);

        if (product is not null) // Already in the basket, just increase quantity
        {
            product.IncreaseQuantity(request.Quantity);
        }
        else // It is a new product on basket, add it.
        {
            basket.Items.Add(new BasketItem(request.ProductId, request.Quantity));
        }

        await _repository.UpdateBasketAsync(basket);

        return new Empty();
    }
}

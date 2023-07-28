using Basket.API.Model;

namespace Basket.API.Infrastructure.Repositories;

public interface IBasketRepository
{
    Task<CustomerBasket?> GetBasketAsync(Guid customerId);

    Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket);
}
namespace Basket.API.Model;

public record CustomerBasket(
    Guid CustomerId,
    List<BasketItem> Items);
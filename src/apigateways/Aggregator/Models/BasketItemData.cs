namespace Aggregator.Models;

public record BasketItemData(int ProductId, int Quantity);

public record CustomerBasket(
    Guid BuyerId,
    List<BasketItem> Items)
{
    public double Total => Items.Sum(p => p.Total);
}

public class BasketItem
{
    public int Id { get; }

    public string Name { get; }

    public string Description { get; }

    public double Price { get; }

    public int Quantity { get; private set; }

    public BasketItem(int id, string name, string description, double price, int quantity)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        Quantity = quantity;
    }

    public double Total => Price * Quantity;
}

public record ProductSummary(int Id, string Name, string Description, double Price);
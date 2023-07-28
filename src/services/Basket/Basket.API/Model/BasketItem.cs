namespace Basket.API.Model;

public class BasketItem
{
    public int Id { get; }

    public int Quantity { get; private set; }

    public BasketItem(int id, int quantity)
    {
        Id = id;
        Quantity = quantity;
    }

    public void IncreaseQuantity(int quantity)
    {
        Quantity += quantity;
    }
}
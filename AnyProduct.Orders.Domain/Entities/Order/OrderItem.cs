using System.ComponentModel.DataAnnotations;

namespace AnyProduct.Orders.Domain.Entities.Order;

public class OrderItem
{
    public Guid Id { get; set; }

    public string? ProductName { get; private set; }

    public string? ImageUrl { get; private set; }

    public decimal? UnitPrice { get; private set; }

    public int Units { get; private set; }

    public Guid ProductId { get; private set; }

    protected OrderItem() { }

    public OrderItem(Guid productId, string productName, decimal unitPrice, string imageUrl, int units = 1)
    {
        if (units <= 0)
        {
            throw new Exception("Invalid number of units");
        }


        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Units = units;
        ImageUrl = imageUrl;
    }
    public OrderItem(Guid productId, int units = 1)
    {
        if (units <= 0)
        {
            throw new Exception("Invalid number of units");
        }


        ProductId = productId;
        Units = units;
    }

    public void AddUnits(int units)
    {
        if (units < 0)
        {
            throw new Exception("Invalid units");
        }

        Units += units;
    }

    public void SetStockDetails(string productName, string  imageUrl, decimal unitPrice)
    {
        ProductName = productName;
        ImageUrl = imageUrl;
        UnitPrice = unitPrice;
    }
}

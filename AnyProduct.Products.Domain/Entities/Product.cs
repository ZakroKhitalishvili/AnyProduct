using AnyProduct.Products.Domain.Events;
using System.ComponentModel.DataAnnotations;

namespace AnyProduct.Products.Domain.Entities;

public class Product : AggregateRoot
{
    [Required, StringLength(200)]
    public string Name { get; private set; }

    [Required]
    public int Amount { get; private set; }
    [Required]
    public int ReservedAmount { get; private set; }

    [Required]
    public decimal Price { get; private set; }

    public string Image { get; private set; }

    [Required]
    public DateTime CreatedDate { get; private set; }

    public DateTime UpdatedDate { get; private set; }

    [Required]
    public ICollection<Guid> ProductCategoryIds { get; private set; }

    public Product(string name, int amount, decimal price, string image, DateTime createdDate, ICollection<Guid> productCategoryIds)
    {
        Name = name;
        Image = image;
        Amount = amount;
        Price = price;
        CreatedDate = createdDate;
        AggregateId = Guid.NewGuid();
        ProductCategoryIds = productCategoryIds;
        ReservedAmount = 0;
        AddEvent(new ProductAddedDomainEvent(this));
    }

    public void Update(string name, int amount, decimal price, string image, DateTime updateDate, ICollection<Guid> productCategoryIds)
    {
        Name = name;
        Image = image;
        Amount = amount;
        Price = price;
        UpdatedDate = updateDate;
        ProductCategoryIds = productCategoryIds;
        AddEvent(new ProductUpdatedDomainEvent(this));
    }

    public void Reserve(int units)
    {
        if (units <= 0)
        {
            throw new ArgumentException(nameof(units), "Value cannot be zero or negative");
        }

        if (Amount < units)
        {
            throw new Exception("Not enough products to reserve");
        }

        Amount -= units;
        ReservedAmount += units;
    }

    public void RemoveReservation(int units)
    {
        if (units <= 0)
        {
            throw new ArgumentException(nameof(units), "Value cannot be zero or negative");
        }

        if (ReservedAmount < units)
        {
            throw new Exception("Not enough reserved products to remove");
        }

        Amount += units;
        ReservedAmount -= units;

    }

    public void ShipReservetion(int units)
    {
        if (units <= 0)
        {
            throw new ArgumentException(nameof(units), "Value cannot be zero or negative");
        }

        if (ReservedAmount < units)
        {
            throw new Exception("Not enough reserved products to ship");
        }

        ReservedAmount -= units;

    }
}

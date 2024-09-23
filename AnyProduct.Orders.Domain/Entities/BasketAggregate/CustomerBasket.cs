
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace AnyProduct.Orders.Domain.Entities.BasketAggregate;

public class CustomerBasket : AggregateRoot
{
    [Required]
    public string BuyerId { get; private set; }

    public Collection<BasketItem> Items { get; private set; } = [];

    public CustomerBasket() { }

    public CustomerBasket(string customerId)
    {
        BuyerId = customerId;
        AggregateId = Guid.NewGuid();
    }

    public void AddOrUpdateItem(Guid productId, int units, DateTime createDate)
    {
        var item = Items.SingleOrDefault(x => x.ProductId == productId);

        if (item is null)
        {
            Items.Add(new BasketItem
            {
                ProductId = productId,
                CreatedDate = createDate,
                Units = units,
                Id = Guid.NewGuid(),
            });
        }
        else
        {
            item.Units = units;
        }
    }

    public void ClearItems()
    {
        Items.Clear();
    }
}



using AnyProduct.Products.Domain.Entities;

namespace AnyProduct.Products.Domain.Events;

public class ProductUpdatedDomainEvent : IDomainEvent
{
    public Product Product { get; }

    public ProductUpdatedDomainEvent(Product product)
    {
        Product = product;
    }
}

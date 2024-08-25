

using AnyProduct.Products.Domain.Entities;

namespace AnyProduct.Products.Domain.Events;

public class ProductAddedDomainEvent : IDomainEvent
{
    public Product Product { get; }

    public ProductAddedDomainEvent(Product product)
    {
        Product = product;
    }
}

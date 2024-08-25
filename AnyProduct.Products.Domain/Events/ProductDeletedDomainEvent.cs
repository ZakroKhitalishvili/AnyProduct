
using AnyProduct.Products.Domain.Entities;

namespace AnyProduct.Products.Domain.Events;

public class ProductDeletedDomainEvent : IDomainEvent
{
    public Product Product { get; }

    public ProductDeletedDomainEvent(Product product)
    {
        Product = product;
    }
}

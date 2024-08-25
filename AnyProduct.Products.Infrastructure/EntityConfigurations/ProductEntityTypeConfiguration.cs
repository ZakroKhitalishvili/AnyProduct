using AnyProduct.Products.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnyProduct.Products.Infrastructure.EntityConfigurations;

public class ProductEntityTypeConfiguration
    : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> buyerConfiguration)
    {
        buyerConfiguration.ToTable("products");

        buyerConfiguration.Ignore(b => b.DomainEvents);

        buyerConfiguration.HasKey(b => b.AggregateId);

        buyerConfiguration.HasIndex(b => b.AggregateId)
            .IsUnique(true);

        buyerConfiguration.PrimitiveCollection(b => b.ProductCategoryIds);
    }
}

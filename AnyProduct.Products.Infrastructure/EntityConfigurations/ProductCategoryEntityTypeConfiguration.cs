using AnyProduct.Products.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnyProduct.Products.Infrastructure.EntityConfigurations;

public class ProductCategoryEntityTypeConfiguration
    : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> buyerConfiguration)
    {
        buyerConfiguration.ToTable("product_categories");

        buyerConfiguration.Ignore(b => b.DomainEvents);

        buyerConfiguration.HasKey(b => b.AggregateId);

        buyerConfiguration.HasIndex(b => b.AggregateId)
            .IsUnique(true);
    }
}

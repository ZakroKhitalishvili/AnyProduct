using AnyProduct.Products.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace AnyProduct.Products.Infrastructure.EntityConfigurations;

public class ProductEntityTypeConfiguration
    : IEntityTypeConfiguration<Product>
{
    public void Configure([NotNull] EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");

        builder.Ignore(b => b.DomainEvents);

        builder.HasKey(b => b.AggregateId);

        builder.HasIndex(b => b.AggregateId)
            .IsUnique(true);

        builder.PrimitiveCollection(b => b.ProductCategoryIds);
    }
}

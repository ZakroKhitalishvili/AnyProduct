using AnyProduct.Products.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace AnyProduct.Products.Infrastructure.EntityConfigurations;

public class ProductCategoryEntityTypeConfiguration
    : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure([NotNull] EntityTypeBuilder<ProductCategory> builder)
    {
        builder.ToTable("product_categories");

        builder.Ignore(b => b.DomainEvents);

        builder.HasKey(b => b.AggregateId);

        builder.HasIndex(b => b.AggregateId)
            .IsUnique(true);
    }
}

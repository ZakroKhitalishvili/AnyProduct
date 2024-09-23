
using AnyProduct.Orders.Domain.Entities.BasketAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace AnyProduct.Orders.Infrastructure.EntityConfigurations;

public class BasketItemEntityTypeConfiguration
    : IEntityTypeConfiguration<BasketItem>
{
    public void Configure([NotNull] EntityTypeBuilder<BasketItem> builder)
    {
        builder.ToTable("basket_items");

        builder.HasKey(b => b.Id);

    }
}

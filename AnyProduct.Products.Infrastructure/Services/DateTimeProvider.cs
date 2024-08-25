
using AnyProduct.Products.Domain.Services;

namespace AnyProduct.Products.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now { get => DateTime.UtcNow; }
}

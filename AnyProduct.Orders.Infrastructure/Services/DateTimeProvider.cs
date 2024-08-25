using AnyProduct.Orders.Domain.Services;

namespace AnyProduct.Orders.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now { get => DateTime.UtcNow; }
}

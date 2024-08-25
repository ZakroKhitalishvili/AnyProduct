namespace AnyProduct.Orders.Domain.Services;

public interface IDateTimeProvider
{
    DateTime Now { get; }
}

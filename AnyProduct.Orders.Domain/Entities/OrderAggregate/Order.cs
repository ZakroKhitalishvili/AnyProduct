using AnyProduct.Orders.Domain.Entities.BuyerAggregate;
using AnyProduct.Orders.Domain.Events;
using System.ComponentModel.DataAnnotations;

namespace AnyProduct.Orders.Domain.Entities.OrderAggregate;

public class Order
    : AggregateRoot
{
    public DateTime OrderDate { get; private set; }

    [Required]
    public AddressValueObject Address { get; private set; }

    public string? BuyerId { get; private set; }

    public Guid? PaymentId { get; private set; }

    public OrderStatus OrderStatus { get; private set; }

    public string Description { get; private set; }


    private readonly List<OrderItem> _orderItems;

    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public string? PaymentMethodId { get; private set; }


    protected Order()
    {
        _orderItems = new List<OrderItem>();
        AggregateId = Guid.NewGuid();
    }

    public Order(string userId, string userName, AddressValueObject address, CardType cardType, string cardNumber, string cardSecurityNumber,
            string cardHolderName, DateTime cardExpiration, string? paymentMethodId = null) : this()
    {
        BuyerId = userId;
        PaymentMethodId = paymentMethodId;
        OrderStatus = OrderStatus.Started;
        OrderDate = DateTime.UtcNow;
        Address = address;


        var orderStartedDomainEvent = new OrderStartedDomainEvent(this, userId, userName, cardType,
                                                             cardNumber, cardSecurityNumber,
                                                             cardHolderName, cardExpiration);

        this.AddEvent(orderStartedDomainEvent);

        Description = "Processing order just started.";
    }

    public void AddOrderItem(Guid productId, int units = 1)
    {
        var existingOrderForProduct = _orderItems.SingleOrDefault(o => o.ProductId == productId);

        if (existingOrderForProduct != null)
        {
            existingOrderForProduct.AddUnits(units);
        }
        else
        {
            var orderItem = new OrderItem(productId, units);
            _orderItems.Add(orderItem);
        }
    }

    public void UpdateStockDetailsForItem(Guid productId, string productName, decimal unitPrice, Uri? imageUrl)
    {
        var existingOrderForProduct = _orderItems.SingleOrDefault(o => o.ProductId == productId);

        if (existingOrderForProduct != null)
        {
            existingOrderForProduct.SetStockDetails(productName, imageUrl, unitPrice);
        }
    }

    public void SetPaidStatus(Guid paymentId)
    {
        if (OrderStatus == OrderStatus.Started)
        {
            AddEvent(new OrderPaidDomainEvent(AggregateId, OrderItems.ToArray()));

            OrderStatus = OrderStatus.Paid;
            PaymentId = paymentId;
            Description = "The payment was performed at simulation";
        }
    }

    public void SetShippedStatus()
    {
        if (OrderStatus != OrderStatus.Paid)
        {
            StatusChangeException(OrderStatus.Shipped);
        }

        OrderStatus = OrderStatus.Shipped;
        Description = "The order was shipped.";
        AddEvent(new OrderShippedDomainEvent(this));
    }

    public void SetCancelledStatus()
    {
        if (OrderStatus == OrderStatus.Paid ||
            OrderStatus == OrderStatus.Shipped)
        {
            StatusChangeException(OrderStatus.Cancelled);
        }

        OrderStatus = OrderStatus.Cancelled;
        Description = $"The order was cancelled.";
        AddEvent(new OrderCancelledDomainEvent(this));
    }

    public void SetCancelledStatusWhenPaymentRejected()
    {
        if (OrderStatus == OrderStatus.Paid ||
            OrderStatus == OrderStatus.Shipped)
        {
            StatusChangeException(OrderStatus.Cancelled);
        }

        OrderStatus = OrderStatus.Cancelled;
        Description = $"Payment failed.";
        AddEvent(new OrderCancelledDomainEvent(this));
    }

    public void SetCancelledStatusWhenStockIsRejected(IEnumerable<Guid> orderStockRejectedItems)
    {
        if (OrderStatus == OrderStatus.Started)
        {
            OrderStatus = OrderStatus.Cancelled;

            var itemsStockRejectedProductNames = OrderItems
                .Where(c => orderStockRejectedItems.Contains(c.ProductId))
                .Select(c => c.ProductName);

            var itemsStockRejectedDescription = string.Join(", ", itemsStockRejectedProductNames);
            Description = $"The product items don't have stock: ({itemsStockRejectedDescription}).";
        }
    }

    public void SetPaymentMethod(string paymentMethodId)
    {
        PaymentMethodId = paymentMethodId;
    }

    private void StatusChangeException(OrderStatus orderStatusToChange)
    {
        throw new InvalidOperationException($"Is not possible to change the order status from {OrderStatus} to {orderStatusToChange}.");
    }

    public decimal GetTotal()
    {
        if (OrderStatus != OrderStatus.Started && OrderStatus != OrderStatus.Shipped)
        {
            throw new InvalidOperationException("Cannot calculate total due to lack of provided information");
        }

        return _orderItems.Sum(o => o.Units * o.UnitPrice!.Value);
    }
}

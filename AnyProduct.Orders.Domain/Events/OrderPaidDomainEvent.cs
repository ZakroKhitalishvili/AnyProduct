﻿using AnyProduct.Orders.Domain.Entities.Order;

namespace AnyProduct.Orders.Domain.Events;
public record class OrderPaidDomainEvent(Guid OrderId, ICollection<OrderItem> OrderItems) : IDomainEvent;
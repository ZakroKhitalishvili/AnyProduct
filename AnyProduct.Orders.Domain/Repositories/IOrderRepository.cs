using AnyProduct.Orders.Domain.Entities;
using AnyProduct.Orders.Domain.Entities.OrderAggregate;
using System;

namespace AnyProduct.Orders.Domain.Repositories;

public interface IOrderRepository
{
    Order Add(Order order);
    void Update(Order order);
    Task<Order?> FindByIdAsync(Guid id);
    ICollection<Order> GetList(out int totalSize, string? customerId, int page = 1, int pageSize = 10);
}

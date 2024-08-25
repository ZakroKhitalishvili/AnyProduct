using AnyProduct.Orders.Domain.Entities.Order;
using AnyProduct.Orders.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AnyProduct.Orders.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderContext _context;

    public OrderRepository(OrderContext context)
    {
        _context = context;
    }

    public Order Add(Order order)
    {
        return _context.Orders.Add(order).Entity;
    }

    public async Task<Order?> FindByIdAsync(Guid id)
    {
        // we fetch an order considering an update lock which avoids us to having concurrency issues like Cancelling and Paying same time
        var order = await _context.Orders
            .FromSql($"SELECT * FROM orders.\"orders\" WHERE \"AggregateId\" = {id} FOR UPDATE").SingleOrDefaultAsync();

        if (order != null)
        {
            await _context.Entry(order)
                .Collection(i => i.OrderItems).LoadAsync();
        }

        return order;
    }

    public void Update(Order order)
    {

        var dbEntry = _context.Entry(order);

        if (dbEntry.State == EntityState.Added)
        {
            return;
        }

        _context.Entry(order).State = EntityState.Modified;
    }

    public ICollection<Order> GetList(out int totalSize, string customerId, int page = 1, int pageSize = 10)
    {
        var query = _context.Orders.AsNoTracking();


        if (customerId is { })
        {
            query = query.Where(x => x.BuyerId == customerId);
        }

        totalSize = query.Count();
        return query.OrderByDescending(x => x.OrderDate)
            .Include(x => x.OrderItems)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }
}

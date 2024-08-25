using AnyProduct.Orders.Domain.Entities.Balance;
using AnyProduct.Orders.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AnyProduct.Orders.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly OrderContext _context;

    public PaymentRepository(OrderContext context)
    {
        _context = context;
    }

    public Payment Add(Payment payment)
    {
        return _context.Payments.Add(payment).Entity;
    }

    public async Task<Payment?> FindByIdAsync(Guid id)
    {
        return await _context.Payments.FindAsync(id);
    }

    public void Update(Payment payment)
    {
        _context.Entry(payment).State = EntityState.Modified;
    }

    public ICollection<Payment> GetList(out int totalSize, string customerId, int page = 1, int pageSize = 10)
    {
        var query = _context.Payments.AsNoTracking();


        if (customerId is { })
        {
            query = query.Where(x => x.BuyerId == customerId);
        }

        totalSize = query.Count();
        return query.OrderByDescending(x => x.PaymentDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }
}

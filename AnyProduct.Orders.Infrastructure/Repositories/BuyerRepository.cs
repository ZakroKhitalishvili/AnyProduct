
using AnyProduct.Orders.Domain.Entities.Buyer;
using AnyProduct.Orders.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AnyProduct.Orders.Infrastructure.Repositories;

public class BuyerRepository : IBuyerRepository
{
    private readonly OrderContext _context;

    public BuyerRepository(OrderContext context)
    {
        _context = context;
    }

    public Buyer Add(Buyer order)
    {
        return _context.Add(order).Entity;
    }

    public async Task<Buyer?> FindByCustomerIdAsync(string id)
    {
        var buyer = await _context.Buyers
            .Include(b => b.PaymentMethods)
            .Where(b => b.IdentityId == id)
            .SingleOrDefaultAsync();

        return buyer;
    }

    public void Update(Buyer buyer)
    {
        var dbEntry = _context.Entry(buyer);

        if (dbEntry.State == EntityState.Added)
        {
            return;
        }

        dbEntry.State = EntityState.Modified;
    }
}

using AnyProduct.Orders.Domain.Entities.BasketAggregate;
using AnyProduct.Orders.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AnyProduct.Orders.Infrastructure.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly OrderContext _context;

    public BasketRepository(OrderContext context)
    {
        _context = context;
    }

    public CustomerBasket Add(CustomerBasket basket)
    {
        return _context.Baskets.Add(basket).Entity;
    }

    public async Task<CustomerBasket?> FindByCustomerIdAsync(string customerId)
    {
        var basket = await _context.Baskets.SingleOrDefaultAsync(x => x.BuyerId == customerId);

        if (basket != null)
        {
            await _context.Entry(basket)
                .Collection(i => i.Items).LoadAsync();
        }

        return basket;
    }

    public void Update(CustomerBasket basket)
    {
        _context.Entry(basket).State = EntityState.Modified;
    }
}

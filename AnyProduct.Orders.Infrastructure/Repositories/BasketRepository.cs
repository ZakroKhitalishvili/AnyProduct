using AnyProduct.Orders.Domain.Entities.Basket;
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

    public async Task<CustomerBasket?> FindByCustomerIdAsync(string id)
    {
        var basket = await _context.Baskets.SingleOrDefaultAsync(x => x.BuyerId == id);

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

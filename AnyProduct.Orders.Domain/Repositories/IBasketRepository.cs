
using AnyProduct.Orders.Domain.Entities.BasketAggregate;

namespace AnyProduct.Orders.Domain.Repositories;

public interface IBasketRepository
{
    CustomerBasket Add(CustomerBasket basket);
    void Update(CustomerBasket basket);
    Task<CustomerBasket?> FindByCustomerIdAsync(string customerId);
}

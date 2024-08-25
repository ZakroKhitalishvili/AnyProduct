
using AnyProduct.Orders.Domain.Entities.Basket;

namespace AnyProduct.Orders.Domain.Repositories;

public interface IBasketRepository
{
    CustomerBasket Add(CustomerBasket basket);
    void Update(CustomerBasket order);
    Task<CustomerBasket> FindByCustomerIdAsync(string customerId);
}

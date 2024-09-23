
using AnyProduct.Orders.Domain.Entities.BuyerAggregate;

namespace AnyProduct.Orders.Domain.Repositories;

public interface IBuyerRepository
{
    Buyer Add(Buyer buyer);
    void Update(Buyer buyer);
    Task<Buyer?> FindByCustomerIdAsync(string id);
}

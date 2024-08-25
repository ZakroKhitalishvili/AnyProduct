
using AnyProduct.Orders.Domain.Entities.Buyer;

namespace AnyProduct.Orders.Domain.Repositories;

public interface IBuyerRepository
{
    Buyer Add(Buyer order);
    void Update(Buyer order);
    Task<Buyer> FindByCustomerIdAsync(string id);
}

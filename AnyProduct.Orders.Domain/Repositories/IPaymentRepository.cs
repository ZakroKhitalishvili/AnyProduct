
using AnyProduct.Orders.Domain.Entities.Balance;

namespace AnyProduct.Orders.Domain.Repositories;

public interface IPaymentRepository
{
    Payment Add(Payment account);
    void Update(Payment account);
    Task<Payment> FindByIdAsync(Guid id);
    ICollection<Payment> GetList(out int totalSize, string customerId, int page = 1, int pageSize = 10);
}

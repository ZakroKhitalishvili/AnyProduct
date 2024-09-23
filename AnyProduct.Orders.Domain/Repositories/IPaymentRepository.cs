
using AnyProduct.Orders.Domain.Entities.PaymentAggregate;

namespace AnyProduct.Orders.Domain.Repositories;

public interface IPaymentRepository
{
    Payment Add(Payment payment);
    void Update(Payment payment);
    Task<Payment?> FindByIdAsync(Guid id);
    ICollection<Payment> GetList(out int totalSize, string? customerId, int page = 1, int pageSize = 10);
}

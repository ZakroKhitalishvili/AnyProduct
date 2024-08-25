namespace AnyProduct.Orders.Application.Services;

public interface IUnitOfWork : IDisposable
{
    Task<string> BeginAsync();
    Task CommitAsync();
    void Rollback();
    bool IsActive { get; }
}




using AnyProduct.Orders.Application.Services;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using Microsoft.EntityFrameworkCore;
using AnyProduct.Orders.Infrastructure;
namespace AnyProduct.Orders.Infrastructure.Services;

public class UnitOfWork : IUnitOfWork
{
    private OrderContext _context;
    private IDbContextTransaction? _currentTransaction;

    public UnitOfWork(OrderContext context)
    {
        _context = context;
    }

    public bool IsActive => _currentTransaction is not null;

    public async Task<string> BeginAsync()
    {
        if (IsActive) throw new Exception("Unit of work is already active");

        _currentTransaction = await _context.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead);

        return _currentTransaction.TransactionId.ToString();
    }

    public async Task CommitAsync()
    {
        if (_currentTransaction == null) throw new ArgumentNullException(nameof(_currentTransaction));

        try
        {
            await _context.SaveChangesAsync();
            await _currentTransaction.CommitAsync();
        }
        catch (Exception ex)
        {
            Rollback();
            throw;
        }
        finally
        {
            if (IsActive)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void Dispose()
    {
        _currentTransaction?.Dispose();
    }

    public void Rollback()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (IsActive)
            {
                _currentTransaction?.Dispose();
                _currentTransaction = null;
            }
        }
    }

}

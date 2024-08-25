
using AnyProduct.Products.Application.Services;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using Microsoft.EntityFrameworkCore;
namespace AnyProduct.Products.Infrastructure.Services;

public class UnitOfWork : IUnitOfWork
{
    private ProductContext _context;
    private IDbContextTransaction? _currentTransaction;

    public UnitOfWork(ProductContext context)
    {
        _context = context;
    }

    public bool IsActive => _currentTransaction is not null;

    public async Task<string> BeginAsync()
    {
        if (IsActive) throw new Exception("Unit of work is already active");

        _currentTransaction = await _context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

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
        catch
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

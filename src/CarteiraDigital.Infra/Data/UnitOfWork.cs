using CarteiraDigital.Core.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace CarteiraDigital.Infra.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly CarteiraDigitalDbContext _context;
    private IDbContextTransaction _currentTransaction;

    public UnitOfWork(CarteiraDigitalDbContext context)
    {
        _context = context;
    }

    public bool HasActiveTransaction => _currentTransaction != null;

    public async Task BeginTransactionAsync()
    {
        if (_currentTransaction != null)
            throw new InvalidOperationException("A transaction is already in progress");

        _currentTransaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        if (_currentTransaction == null)
            throw new InvalidOperationException("No transaction to commit");

        try
        {
            await _context.SaveChangesAsync();
            await _currentTransaction.CommitAsync();
        }
        catch
        {
            await RollbackAsync();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public async Task RollbackAsync()
    {
        if (_currentTransaction == null)
            throw new InvalidOperationException("No transaction to rollback");

        try
        {
            await _currentTransaction.RollbackAsync();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _currentTransaction?.Dispose();
        _context.Dispose();
    }
}

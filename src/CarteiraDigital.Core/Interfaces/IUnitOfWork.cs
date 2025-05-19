namespace CarteiraDigital.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
    Task<int> SaveChangesAsync();
    bool HasActiveTransaction { get; }
}
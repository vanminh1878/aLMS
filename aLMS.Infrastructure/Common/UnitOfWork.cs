using aLMS.Application.Common.Interfaces;
using aLMS.Infrastructure.Common;

public class UnitOfWork : IUnitOfWork
{
    private readonly aLMSDbContext _context;

    public UnitOfWork(aLMSDbContext context)
    {
        _context = context;
    }

    public async Task BeginTransactionAsync(CancellationToken ct = default)
        => await _context.Database.BeginTransactionAsync(ct);

    public async Task CommitTransactionAsync(CancellationToken ct = default)
        => await _context.Database.CurrentTransaction!.CommitAsync(ct);


    public async Task RollbackTransactionAsync(CancellationToken ct = default)
        => await _context.Database.CurrentTransaction!.RollbackAsync(ct);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);
}
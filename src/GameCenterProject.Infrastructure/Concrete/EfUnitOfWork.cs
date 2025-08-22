using GameCenterProject.Infrastructure.Persistence;
using GameCenterProject.Infrastructure.Abstract;

namespace GameCenterProject.Infrastructure.Concrete;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;
    public EfUnitOfWork(AppDbContext db) => _db = db;
    public Task<int> SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
}
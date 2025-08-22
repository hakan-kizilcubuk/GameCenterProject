using GameCenterProject.Entities;
using GameCenterProject.Infrastructure.Persistence;
using GameCenterProject.Infrastructure.Abstract;
using Microsoft.EntityFrameworkCore;

namespace GameCenterProject.Infrastructure.Concrete;

public class EfCartRepository : ICartRepository
{
    private readonly AppDbContext _db;
    public EfCartRepository(AppDbContext db) => _db = db;

    public Task<Cart?> FindByUserAsync(string userId, CancellationToken ct = default)
        => _db.Carts
               .Include(c => c.Games)
               .FirstOrDefaultAsync(c => c.UserId == userId, ct);

    public Task AddAsync(Cart cart, CancellationToken ct = default)
        => _db.Carts.AddAsync(cart, ct).AsTask();

    public async Task RemoveAsync(Guid cartId, CancellationToken ct = default)
    {
        var entity = await _db.Carts.FirstOrDefaultAsync(c => c.Id == cartId, ct);
        if (entity is not null) _db.Carts.Remove(entity);
    }
}
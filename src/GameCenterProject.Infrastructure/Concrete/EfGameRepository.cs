using GameCenterProject.Entities;
using GameCenterProject.Infrastructure.Persistence;
using GameCenterProject.Infrastructure.Abstract;
using Microsoft.EntityFrameworkCore;

namespace GameCenterProject.Infrastructure.Concrete;

public class EfGameRepository : IGameRepository
{
    private readonly AppDbContext _db;
    public EfGameRepository(AppDbContext db) => _db = db;

    public Task<Game?> FindAsync(Guid id, CancellationToken ct = default)
        => _db.Games.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id, ct);

    public async Task<List<Game>> SearchAsync(string? q, int skip, int take, CancellationToken ct = default)
    {
        var query = _db.Games.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(g => EF.Functions.Like(g.Title, $"%{q}%") || EF.Functions.Like(g.Description, $"%{q}%"));
        return await query.OrderBy(g => g.Title).Skip(skip).Take(take).ToListAsync(ct);
    }

    public Task AddAsync(Game game, CancellationToken ct = default)
        => _db.Games.AddAsync(game, ct).AsTask();
}
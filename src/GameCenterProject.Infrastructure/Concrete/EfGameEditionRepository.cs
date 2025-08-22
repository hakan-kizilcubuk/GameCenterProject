using GameCenterProject.Entities;
using GameCenterProject.Infrastructure.Persistence;
using GameCenterProject.Infrastructure.Abstract;
using Microsoft.EntityFrameworkCore;

namespace GameCenterProject.Infrastructure.Concrete;

public class EfGameEditionRepository : IGameEditionRepository
{
    private readonly AppDbContext _db;
    public EfGameEditionRepository(AppDbContext db) => _db = db;

    public Task<List<GameEdition>> ListForGameAsync(Guid gameId, CancellationToken ct = default)
        => _db.GameEditions.AsNoTracking().Where(e => EF.Property<Guid>(e, "GameId") == gameId).ToListAsync(ct);

    public async Task AddAsync(Guid gameId, GameEdition edition, CancellationToken ct = default)
    {
        // set shadow FK
        _db.Entry(edition).Property("GameId").CurrentValue = gameId;
        await _db.GameEditions.AddAsync(edition, ct);
    }
}

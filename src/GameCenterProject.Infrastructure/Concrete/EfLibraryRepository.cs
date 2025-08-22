using GameCenterProject.Entities;
using GameCenterProject.Infrastructure.Persistence;
using GameCenterProject.Infrastructure.Abstract;
using Microsoft.EntityFrameworkCore;

namespace GameCenterProject.Infrastructure.Concrete;

public class EfLibraryRepository : ILibraryRepository
{
    private readonly AppDbContext _db;
    public EfLibraryRepository(AppDbContext db) => _db = db;

    public Task<Library?> FindAsync(string userId, CancellationToken ct = default)
        => _db.Libraries.Include(l => l.OwnedGames).FirstOrDefaultAsync(l => l.UserId == userId, ct);

    public async Task<List<Game>> ListGamesAsync(string userId, CancellationToken ct = default)
    {
        var lib = await _db.Libraries.Include(l => l.OwnedGames).FirstOrDefaultAsync(l => l.UserId == userId, ct);
        return lib?.OwnedGames?.ToList() ?? new List<Game>();
    }

    public Task AddAsync(Library library, CancellationToken ct = default)
        => _db.Libraries.AddAsync(library, ct).AsTask();
}

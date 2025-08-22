using GameCenterProject.Application.Abstractions;
using GameCenterProject.Entities;
using GameCenterProject.Infrastructure.Abstract;


namespace GameCenterProject.Application.Services;

public sealed class CatalogService : ICatalogService
{
    private readonly IGameRepository _games;
    public CatalogService(IGameRepository games) => _games = games;

    public async Task<IReadOnlyList<Game>> SearchAsync(string? q, int page = 1, int pageSize = 20, CancellationToken ct = default)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);
        var skip = (page - 1) * pageSize;
        return await _games.SearchAsync(q, skip, pageSize, ct);
    }

    public Task<Game?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _games.FindAsync(id, ct);
}
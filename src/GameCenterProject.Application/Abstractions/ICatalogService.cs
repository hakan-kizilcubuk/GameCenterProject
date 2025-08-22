using GameCenterProject.Entities;

namespace GameCenterProject.Application.Services;

public interface ICatalogService
{
    Task<IReadOnlyList<Game>> SearchAsync(string? q, int page = 1, int pageSize = 20, CancellationToken ct = default);
    Task<Game?> GetByIdAsync(Guid id, CancellationToken ct = default);
}
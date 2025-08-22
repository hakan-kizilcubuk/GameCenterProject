using GameCenterProject.Entities;

public interface ILibraryService
{
    Task<Library> GetOrCreateAsync(string userId, CancellationToken ct = default);
    Task<Library> AddGameAsync(string userId, Guid gameId, CancellationToken ct = default);
    Task<Library> PurchaseCartAsync(string userId, CancellationToken ct = default);
}

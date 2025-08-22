using GameCenterProject.Entities;

namespace GameCenterProject.Application.Abstractions
{
    public interface ICartService
    {
        Task<Cart> GetOrCreateAsync(string userId, CancellationToken ct = default);
        Task<Cart> AddGameAsync(string userId, Guid gameId, CancellationToken ct = default);
        Task<Cart> RemoveGameAsync(string userId, Guid gameId, CancellationToken ct = default);
        Task ClearAsync(string userId, CancellationToken ct = default);
    }
}
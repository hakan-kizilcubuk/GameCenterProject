using GameCenterProject.Entities;

namespace GameCenterProject.Infrastructure.Abstract
{

    public interface IGameEditionRepository
    {
        Task<List<GameEdition>> ListForGameAsync(Guid gameId, CancellationToken ct = default);
        Task AddAsync(Guid gameId, GameEdition edition, CancellationToken ct = default);
    }
}
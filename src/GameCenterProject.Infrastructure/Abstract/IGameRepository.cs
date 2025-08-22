using System;
using System.Collections.Generic;
using System.Text;
using GameCenterProject.Entities;
namespace GameCenterProject.Infrastructure.Abstract
{
    public interface IGameRepository
    {
            Task<Game?> FindAsync(Guid id, CancellationToken ct = default);
            Task<List<Game>> SearchAsync(string? q, int skip, int take, CancellationToken ct = default);
            Task AddAsync(Game game, CancellationToken ct = default);
    }
}
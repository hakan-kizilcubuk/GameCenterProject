using System;
using System.Collections.Generic;
using System.Text;
using GameCenterProject.Entities;

namespace GameCenterProject.Infrastructure.Abstract
{
    public interface ILibraryRepository
    {
        Task<Library?> FindAsync(string userId, CancellationToken ct = default);
        Task<List<Game>> ListGamesAsync(string userId, CancellationToken ct = default);
        Task AddAsync(Library library, CancellationToken ct = default);
    }
}
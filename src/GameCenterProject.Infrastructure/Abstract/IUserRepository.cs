using System;
using System.Threading;
using System.Threading.Tasks;
using GameCenterProject.Entities;

namespace GameCenterProject.Infrastructure.Abstract
{
    public interface IUserRepository
    {
        Task<User?> FindAsync(string userId, CancellationToken ct = default);
        Task<User?> FindByEmailAsync(string email, CancellationToken ct = default);
    }
}

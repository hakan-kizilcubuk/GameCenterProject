using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GameCenterProject.Entities;

namespace GameCenterProject.Infrastructure.Abstract
{
    public interface IDiscountRepository
    {
        Task<Discount?> FindAsync(Guid id, CancellationToken ct = default);
        Task<List<Discount>> ListByGameAsync(Guid gameId, CancellationToken ct = default);
        Task AddAsync(Discount discount, CancellationToken ct = default);
        Task RemoveAsync(Guid id, CancellationToken ct = default);
        Task<List<Discount>> ListActiveAsync(DateTime now, CancellationToken ct = default);
    }
}

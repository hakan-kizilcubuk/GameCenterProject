using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GameCenterProject.Entities;

namespace GameCenterProject.Application.Abstractions
{
    public interface IDiscountService
    {
        Task<Discount?> GetAsync(Guid id, CancellationToken ct = default);
        Task<List<Discount>> ListByGameAsync(Guid gameId, CancellationToken ct = default);
        Task<List<Discount>> ListActiveAsync(DateTime now, CancellationToken ct = default);
        Task<Discount> CreateAsync(Discount discount, string adminUserId, CancellationToken ct = default);
        Task RemoveAsync(Guid id, string adminUserId, CancellationToken ct = default);
    }
}

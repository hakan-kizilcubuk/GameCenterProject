using System;
using System.Collections.Generic;
using System.Text;
using GameCenterProject.Entities;

namespace GameCenterProject.Infrastructure.Abstract
{
    public interface ICartRepository
    {
        Task<Cart?> FindByUserAsync(string userId, CancellationToken ct = default);
        Task AddAsync(Cart cart, CancellationToken ct = default);
        Task RemoveAsync(Guid cartId, CancellationToken ct = default);
    }
}
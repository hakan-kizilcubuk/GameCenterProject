using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GameCenterProject.Application.Abstractions;
using GameCenterProject.Entities;
using GameCenterProject.Infrastructure.Abstract;

namespace GameCenterProject.Application.Services;
public class DiscountService : IDiscountService
{
    private readonly IDiscountRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IUserRepository _users;

    public DiscountService(IDiscountRepository repo, IUnitOfWork uow, IUserRepository users)
    {
        _repo = repo;
        _uow = uow;
        _users = users;
    }

    public Task<Discount?> GetAsync(Guid id, CancellationToken ct = default)
        => _repo.FindAsync(id, ct);

    public Task<List<Discount>> ListByGameAsync(Guid gameId, CancellationToken ct = default)
        => _repo.ListByGameAsync(gameId, ct);

    public Task<List<Discount>> ListActiveAsync(DateTime now, CancellationToken ct = default)
        => _repo.ListActiveAsync(now, ct);

    public async Task<Discount> CreateAsync(Discount discount, string adminUserId, CancellationToken ct = default)
    {
        var user = await _users.FindAsync(adminUserId, ct);
        if (user == null || user.Role != "admin")
            throw new UnauthorizedAccessException("Only admin users can create discounts.");
        discount.CreatedBy = user.Email;
        await _repo.AddAsync(discount, ct);
        await _uow.SaveChangesAsync(ct);
        return discount;
    }

    public async Task RemoveAsync(Guid id, string adminUserId, CancellationToken ct = default)
    {
        var user = await _users.FindAsync(adminUserId, ct);
        if (user == null || user.Role != "admin")
            throw new UnauthorizedAccessException("Only admin users can remove discounts.");
        await _repo.RemoveAsync(id, ct);
        await _uow.SaveChangesAsync(ct);
    }
}

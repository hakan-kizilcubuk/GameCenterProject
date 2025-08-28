using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameCenterProject.Entities;
using GameCenterProject.Infrastructure.Persistence;
using GameCenterProject.Infrastructure.Abstract;
using Microsoft.EntityFrameworkCore;

namespace GameCenterProject.Infrastructure.Concrete;

public class EfDiscountRepository : IDiscountRepository
{
    private readonly AppDbContext _db;
    public EfDiscountRepository(AppDbContext db) => _db = db;

    public Task<Discount?> FindAsync(Guid id, CancellationToken ct = default)
        => _db.Discounts.FirstOrDefaultAsync(d => d.Id == id, ct);

    public Task<List<Discount>> ListByGameAsync(Guid gameId, CancellationToken ct = default)
        => _db.Discounts.Where(d => d.GameId == gameId).ToListAsync(ct);

    public Task AddAsync(Discount discount, CancellationToken ct = default)
        => _db.Discounts.AddAsync(discount, ct).AsTask();

    public async Task RemoveAsync(Guid id, CancellationToken ct = default)
    {
        var d = await _db.Discounts.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (d != null) _db.Discounts.Remove(d);
    }

    public Task<List<Discount>> ListActiveAsync(DateTime now, CancellationToken ct = default)
        => _db.Discounts.Where(d => d.StartDate <= now && d.EndDate >= now).ToListAsync(ct);
}

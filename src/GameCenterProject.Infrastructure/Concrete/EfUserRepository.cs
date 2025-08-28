using System;
using System.Threading;
using System.Threading.Tasks;
using GameCenterProject.Entities;
using GameCenterProject.Infrastructure.Persistence;
using GameCenterProject.Infrastructure.Abstract;
using Microsoft.EntityFrameworkCore;

namespace GameCenterProject.Infrastructure.Concrete;

public class EfUserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    public EfUserRepository(AppDbContext db) => _db = db;

    public Task<User?> FindAsync(string userId, CancellationToken ct = default)
    {
        if(Guid.TryParse(userId, out var guid))
            return _db.Users.FirstOrDefaultAsync(u => u.Id == guid, ct);
        return _db.Users.FirstOrDefaultAsync(u => u.Email == userId, ct);
    }

    public Task<User?> FindByEmailAsync(string email, CancellationToken ct = default)
        => _db.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
}

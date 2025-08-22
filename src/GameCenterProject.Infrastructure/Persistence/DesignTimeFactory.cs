// src/GameCenterProject.Infrastructure/Persistence/DesignTimeFactory.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GameCenterProject.Infrastructure.Persistence;

public class DesignTimeFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var cs = "Server=localhost;Database=GameCenter;Trusted_Connection=True;";
        var opts = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(cs, sql => sql.EnableRetryOnFailure())
            .Options;
        return new AppDbContext(opts);
    }
}

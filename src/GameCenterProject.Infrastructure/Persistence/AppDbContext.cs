using System;
using System.Collections.Generic;
using System.Text;
using GameCenterProject.Entities;
// ...existing code...
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameCenterProject.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Game> Games => Set<Game>();
    public DbSet<GameEdition> GameEditions => Set<GameEdition>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<Library> Libraries => Set<Library>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder b)
{
    // User entity configuration
    b.Entity<User>(e =>
    {
        e.HasKey(x => x.Id);
        e.Property(x => x.Email).IsRequired().HasMaxLength(256);
        e.Property(x => x.DisplayName).HasMaxLength(128);
        e.Property(x => x.PasswordHash).IsRequired();
        e.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
    });

    // ===== Game =====
    b.Entity<Game>(e =>
    {
        e.Property(x => x.Title).IsRequired().HasMaxLength(256);
        e.Property(x => x.Description).HasMaxLength(4000);
        e.OwnsOne(x => x.Price, owned =>
        {
            owned.Property(p => p.Amount).HasColumnName("PriceAmount").HasPrecision(18, 2);
            owned.Property(p => p.Currency).HasColumnName("PriceCurrency").HasMaxLength(3);
        });

        // If you want to persist _editions (field-backed nav):
        e.HasMany(typeof(GameEdition), "_editions")
         .WithOne()
         .HasForeignKey("GameId")
         .OnDelete(DeleteBehavior.Cascade);
    });

    // ===== GameEdition =====
    b.Entity<GameEdition>(e =>
    {
        e.Property(x => x.Name).IsRequired().HasMaxLength(128);
        e.Property(x => x.Description).HasMaxLength(2000);
        e.OwnsOne(x => x.Price, owned =>
        {
            owned.Property(p => p.Amount).HasColumnName("PriceAmount").HasPrecision(18, 2);
            owned.Property(p => p.Currency).HasColumnName("PriceCurrency").HasMaxLength(3);
        });
    });

    // ===== Genre / Tag =====
    b.Entity<Genre>().Property(x => x.Name).IsRequired().HasMaxLength(64);
    b.Entity<Tag>().Property(x => x.Name).IsRequired().HasMaxLength(64);

    // ===== Cart =====
    b.Entity<Cart>(e =>
    {
        e.Property(x => x.UserId).IsRequired().HasMaxLength(128);
        e.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
        e.Property(x => x.UpdatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

        // Money owned type
        e.OwnsOne(x => x.TotalPrice, owned =>
        {
            owned.Property(p => p.Amount).HasColumnName("TotalAmount").HasPrecision(18, 2).HasDefaultValue(0m);
            owned.Property(p => p.Currency).HasColumnName("TotalCurrency").HasMaxLength(3).HasDefaultValue("USD");
        });

        // Owned collection: Ratings
        e.OwnsMany(x => x.Ratings, r =>
        {
            r.ToTable("CartRatings");
            r.WithOwner().HasForeignKey("CartId");
            r.Property<int>("Id");                // key for owned row
            r.HasKey("Id");
            r.Property(x => x.GameId).HasMaxLength(64);
            r.Property(x => x.Rate).IsRequired();
        });

        // Many-to-many: Cart <-> Games (join table CartGames)
        e.HasMany(c => c.Games)
         .WithMany()
         .UsingEntity<Dictionary<string, object>>(
            "CartGames",
            j => j.HasOne<Game>().WithMany().HasForeignKey("GameId").OnDelete(DeleteBehavior.Cascade),
            j => j.HasOne<Cart>().WithMany().HasForeignKey("CartId").OnDelete(DeleteBehavior.Cascade),
            j => j.HasKey("CartId", "GameId")
         );
    });

    // ===== Library (key is UserId) =====
    b.Entity<Library>(e =>
    {
        e.HasKey(x => x.UserId); // IMPORTANT (no Id property)
        e.Property(x => x.UserId).HasMaxLength(128); 
        e.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
        e.Property(x => x.UpdatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

        // Library <-> Games many-to-many
        e.HasMany(l => l.OwnedGames)
         .WithMany()
         .UsingEntity<Dictionary<string, object>>(
            "LibraryGames",
            j => j.HasOne<Game>().WithMany().HasForeignKey("GameId").OnDelete(DeleteBehavior.Cascade),
            j => j.HasOne<Library>().WithMany().HasForeignKey("LibraryUserId").OnDelete(DeleteBehavior.Cascade),
            j =>
            {
                j.HasKey("LibraryUserId", "GameId");
                // Ensure the join column matches Library.UserId size:
                j.Property<string>("LibraryUserId").HasMaxLength(128).HasColumnType("nvarchar(128)");
            });
    });
}

}

using GameCenterProject.Entities;
using GameCenterProject.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameCenterProject.Infrastructure.Persistence.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> b)
    {
        b.ToTable("Carts");
        b.HasKey(c => c.Id);
        b.Property(c => c.UserId).HasMaxLength(128).IsRequired();
        b.Property(c => c.CreatedAt).IsRequired();
        b.Property(c => c.UpdatedAt).IsRequired();

        // TotalPrice as owned Money
        b.OwnsOne(c => c.TotalPrice, m =>
        {
            m.Property(p => p.Amount).HasColumnName("TotalAmount").HasColumnType("decimal(18,2)");
            m.Property(p => p.Currency).HasColumnName("TotalCurrency").HasMaxLength(3);
        });

        // Ratings as owned collection
        b.OwnsMany(c => c.Ratings, r =>
        {
            r.ToTable("CartRatings");
            r.WithOwner().HasForeignKey("CartId");
            r.Property<int>("Id").ValueGeneratedOnAdd();
            r.HasKey("Id");
            r.Property(p => p.GameId).HasMaxLength(64);
            r.Property(p => p.Rate).IsRequired();
        });

        // Many-to-many Cart <-> Games via join table
        b.HasMany(c => c.Games)
         .WithMany()
         .UsingEntity<Dictionary<string, object>>(
            "CartGames",
            j => j.HasOne<Game>().WithMany().HasForeignKey("GameId").OnDelete(DeleteBehavior.Cascade),
            j => j.HasOne<Cart>().WithMany().HasForeignKey("CartId").OnDelete(DeleteBehavior.Cascade),
            j => { j.HasKey("CartId", "GameId"); j.ToTable("CartGames"); });
    }
}

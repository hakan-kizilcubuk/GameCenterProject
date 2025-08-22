using GameCenterProject.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameCenterProject.Infrastructure.Persistence.Configurations;

public class LibraryConfiguration : IEntityTypeConfiguration<Library>
{
    public void Configure(EntityTypeBuilder<Library> b)
    {
        b.ToTable("Libraries");
        // Library has no Id in your model, we use UserId as the PK
        b.HasKey(l => l.UserId);
        b.Property(l => l.UserId).HasMaxLength(128).IsRequired();
        b.Property(l => l.CreatedAt).IsRequired();
        b.Property(l => l.UpdatedAt).IsRequired();

        // Many-to-many Library <-> Games via join table
        b.HasMany(l => l.OwnedGames)
         .WithMany()
         .UsingEntity<Dictionary<string, object>>(
            "LibraryGames",
            j => j.HasOne<Game>().WithMany().HasForeignKey("GameId").OnDelete(DeleteBehavior.Cascade),
            j => j.HasOne<Library>().WithMany().HasForeignKey("LibraryUserId").OnDelete(DeleteBehavior.Cascade),
            j => { j.HasKey("LibraryUserId", "GameId"); j.ToTable("LibraryGames"); });
    }
}
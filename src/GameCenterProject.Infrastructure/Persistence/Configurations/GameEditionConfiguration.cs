using GameCenterProject.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameCenterProject.Infrastructure.Persistence.Configurations;

public class GameEditionConfiguration : IEntityTypeConfiguration<GameEdition>
{
    public void Configure(EntityTypeBuilder<GameEdition> b)
    {
        b.ToTable("GameEditions");
        b.HasKey(e => e.Id);
        b.Property(e => e.Name).HasMaxLength(128).IsRequired();
        b.Property(e => e.Description).HasMaxLength(2000);

        // Price as owned
        b.OwnsOne(e => e.Price, m =>
        {
            m.Property(p => p.Amount).HasColumnName("PriceAmount").HasColumnType("decimal(18,2)");
            m.Property(p => p.Currency).HasColumnName("PriceCurrency").HasMaxLength(3);
        });

        // Shadow FK -> Game (because your class has no GameId property)
        b.Property<Guid>("GameId");
        b.HasOne<Game>()
            .WithMany() // no nav on Game
            .HasForeignKey("GameId")
            .OnDelete(DeleteBehavior.Cascade);

        // Create index for lookups
        b.HasIndex("GameId");
    }
}

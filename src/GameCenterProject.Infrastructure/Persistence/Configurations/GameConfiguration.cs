using GameCenterProject.Entities;
using GameCenterProject.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameCenterProject.Infrastructure.Persistence.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> b)
    {
        b.ToTable("Games");
        b.HasKey(g => g.Id);
        b.Property(g => g.Title).HasMaxLength(256).IsRequired();
        b.Property(g => g.Description).HasMaxLength(4000);
        b.Property(g => g.ReleaseDate); // stored as INT in your model

        // Money (Price) as owned type
        b.OwnsOne(g => g.Price, m =>
        {
            m.Property(p => p.Amount).HasColumnName("PriceAmount").HasColumnType("decimal(18,2)");
            m.Property(p => p.Currency).HasColumnName("PriceCurrency").HasMaxLength(3);
        });

        // NOTE about Editions/Tags/Genres navigations:
        // Your Game class keeps private lists (_editions, _tags, _genres) without public navs.
        // We'll map GameEdition as a separate entity with a shadow FK to GameId;
        // you can query editions via DbContext.GameEditions. If you expose a property
        // like `public IReadOnlyCollection<GameEdition> Editions => _editions.AsReadOnly();`
        // we can switch to a conventional relationship.
    }
}
using GameCenterProject.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameCenterProject.Infrastructure.Persistence.Configurations;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> b)
    {
        b.ToTable("Genres");
        b.HasKey(g => g.Id);
        b.Property(g => g.Name).HasMaxLength(64).IsRequired();
    }
}

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> b)
    {
        b.ToTable("Tags");
        b.HasKey(t => t.Id);
        b.Property(t => t.Name).HasMaxLength(64).IsRequired();
    }
}

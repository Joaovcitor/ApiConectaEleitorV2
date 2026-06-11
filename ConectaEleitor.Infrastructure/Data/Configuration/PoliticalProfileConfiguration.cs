using ConectaEleitor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConectaEleitor.Infrastructure.Configurations;

public class PoliticalProfileConfiguration : IEntityTypeConfiguration<PoliticalProfile>
{
    public void Configure(EntityTypeBuilder<PoliticalProfile> builder)
    {
        builder.HasKey(p => p.PoliticalProfileId);

        builder.Property(p => p.DisplayName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(p => p.Bio)
            .HasMaxLength(1000);

        builder.Property(p => p.PhotoUrl)
            .HasMaxLength(500);

        builder.Property(p => p.BannerUrl)
            .HasMaxLength(500);

        builder.Property(p => p.Office)
            .HasMaxLength(100);

        builder.Property(p => p.Party)
            .HasMaxLength(50);

        builder.Property(p => p.City)
            .HasMaxLength(100);

        builder.Property(p => p.State)
            .HasMaxLength(50);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.HasIndex(p => p.OwnerId);
        builder.HasIndex(p => p.UserId);

        builder.HasIndex(p => new { p.OwnerId, p.UserId })
            .IsUnique();

        builder.HasIndex(p => new { p.City, p.State });
        builder.HasIndex(p => p.IsActive);
        builder.HasIndex(p => p.IsVerified);
    }
}
using ConectaEleitor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConectaEleitor.Infrastructure.Configurations;

public class AssemblymanPostLikeConfiguration : IEntityTypeConfiguration<AssemblymanPostLike>
{
    public void Configure(EntityTypeBuilder<AssemblymanPostLike> builder)
    {
        builder.HasKey(l => l.AssemblymanPostLikeId);

        builder.Property(l => l.CreatedAt)
            .IsRequired();

        builder.HasIndex(l => l.PostId);
        builder.HasIndex(l => l.UserId);

        builder.HasIndex(l => new { l.PostId, l.UserId })
            .IsUnique();
    }
}
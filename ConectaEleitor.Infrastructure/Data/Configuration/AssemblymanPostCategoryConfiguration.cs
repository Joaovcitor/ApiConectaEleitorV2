using ConectaEleitor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConectaEleitor.Infrastructure.Configurations;

public class AssemblymanPostCategoryConfiguration : IEntityTypeConfiguration<AssemblymanPostCategory>
{
    public void Configure(EntityTypeBuilder<AssemblymanPostCategory> builder)
    {
        builder.HasKey(c => c.AssemblymanPostCategoryId);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Description)
            .HasMaxLength(300);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.HasIndex(c => c.OwnerId);

        builder.HasIndex(c => new { c.OwnerId, c.Name })
            .IsUnique();
    }
}
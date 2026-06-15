using ConectaEleitor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConectaEleitor.Infrastructure.Data.Configuration;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(t => t.TagId);
        
        builder.Property(t => t.Name).IsRequired().HasMaxLength(50);
        builder.Property(t => t.Description).HasMaxLength(200);
        builder.Property(t => t.Color).HasMaxLength(7);
        
        builder.HasMany(t => t.TagsCitizens)
            .WithOne(tc => tc.Tag)
            .HasForeignKey(tc => tc.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
using ConectaEleitor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConectaEleitor.Infrastructure.Data.Configuration;

public class TagCitizenConfiguration : IEntityTypeConfiguration<TagCitizen>
{
    public void Configure(EntityTypeBuilder<TagCitizen> builder)
    {
        builder.HasKey(tc => new
        {
            tc.TagId,
            tc.CitizenId
        });

        builder.Property(tc => tc.Reason)
            .HasMaxLength(200);

        builder.HasOne(tc => tc.Tag)
            .WithMany(t => t.TagsCitizens)
            .HasForeignKey(tc => tc.TagId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(tc => tc.Citizen)
            .WithMany(c => c.TagCitizens)
            .HasForeignKey(tc => tc.CitizenId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
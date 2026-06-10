using ConectaEleitor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConectaEleitor.Infrastructure.Data.Configuration;

public class DemandConfiguration : IEntityTypeConfiguration<Demand>
{
    public void Configure(EntityTypeBuilder<Demand> builder)
    {
        builder.HasKey(x => x.DemandId);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.Priority)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.HasOne(x => x.Citizen)
            .WithMany(x => x.Demands)
            .HasForeignKey(x => x.CitizenId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.OwnerId);
        builder.HasIndex(x => x.CitizenId);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.Priority);
        builder.HasIndex(x => x.CreatedAt);
    }
}
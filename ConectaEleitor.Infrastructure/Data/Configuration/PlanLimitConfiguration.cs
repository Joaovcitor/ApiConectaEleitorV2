using ConectaEleitor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConectaEleitor.Infrastructure.Data.Configuration;

public class PlanLimitConfiguration : IEntityTypeConfiguration<PlanLimit>
{
    public void Configure(EntityTypeBuilder<PlanLimit> builder)
    {
        builder.ToTable("PlanLimits");

        builder.HasKey(x => x.PlanLimitId);

        builder.Property(x => x.Type)
            .HasConversion<int>();

        builder.HasIndex(x => new
        {
            x.PlanId,
            x.Type
        }).IsUnique();
    }
}

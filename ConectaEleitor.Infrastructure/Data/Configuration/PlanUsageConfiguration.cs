using ConectaEleitor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConectaEleitor.Infrastructure.Data.Configuration;

public class PlanUsageConfiguration : IEntityTypeConfiguration<PlanUsage>
{
    public void Configure(EntityTypeBuilder<PlanUsage> builder)
    {
        builder.ToTable("PlanUsages");

        builder.HasKey(x => x.PlanUsageId);

        builder.HasIndex(x => x.OwnerId);

        builder.HasIndex(x => new
        {
            x.OwnerId,
            x.Year,
            x.Month
        }).IsUnique();
    }
}
using ConectaEleitor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConectaEleitor.Infrastructure.Data.Configuration;

public class SubscriptionHistoryConfiguration : IEntityTypeConfiguration<SubscriptionHistory>
{
    public void Configure(EntityTypeBuilder<SubscriptionHistory> builder)
    {
        builder.ToTable("SubscriptionHistory");

        builder.HasKey(x => x.SubscriptionHistoryId);

        builder.Property(x => x.Action)
            .HasConversion<int>();

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.HasIndex(x => x.OwnerSubscriptionId);

        builder.HasIndex(x => x.CreatedAt);
    }
}
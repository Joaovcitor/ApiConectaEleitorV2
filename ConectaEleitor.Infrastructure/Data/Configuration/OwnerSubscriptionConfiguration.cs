using ConectaEleitor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConectaEleitor.Infrastructure.Data.Configuration;

public class OwnerSubscriptionConfiguration : IEntityTypeConfiguration<OwnerSubscription>
{
    public void Configure(EntityTypeBuilder<OwnerSubscription> builder)
    {
        builder.ToTable("OwnerSubscriptions");

        builder.HasKey(x => x.OwnerSubscriptionId);

        builder.HasIndex(x => x.OwnerId);

        builder.HasIndex(x => x.Status);

        builder.Property(x => x.Status)
            .HasConversion<int>();

        builder.Property(x => x.BillingCycle)
            .HasConversion<int>();

        builder.Property(x => x.CancelReason)
            .HasMaxLength(500);

        builder.HasOne(x => x.Plan)
            .WithMany(x => x.Subscriptions)
            .HasForeignKey(x => x.PlanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.History)
            .WithOne(x => x.OwnerSubscription)
            .HasForeignKey(x => x.OwnerSubscriptionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

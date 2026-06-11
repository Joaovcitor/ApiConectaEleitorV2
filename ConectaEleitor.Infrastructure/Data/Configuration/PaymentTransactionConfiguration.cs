using ConectaEleitor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConectaEleitor.Infrastructure.Data.Configurations;

public class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
{
    public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
    {
        builder.ToTable("PaymentTransactions");

        builder.HasKey(x => x.PaymentTransactionId);

        builder.Property(x => x.OwnerId)
            .IsRequired();

        builder.Property(x => x.Amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(x => x.Currency)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(x => x.Provider)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.PaymentMethod)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.BillingCycle)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.ExternalPaymentId)
            .HasMaxLength(150);

        builder.Property(x => x.ExternalInvoiceId)
            .HasMaxLength(150);

        builder.Property(x => x.CheckoutUrl)
            .HasMaxLength(1000);

        builder.Property(x => x.PixQrCode);

        builder.Property(x => x.PixQrCodeBase64);

        builder.Property(x => x.BoletoUrl)
            .HasMaxLength(1000);

        builder.Property(x => x.FailureReason)
            .HasMaxLength(1000);

        builder.Property(x => x.MetadataJson)
            .HasColumnType("jsonb");

        builder.HasOne(x => x.OwnerSubscription)
            .WithMany(x => x.Payments)
            .HasForeignKey(x => x.OwnerSubscriptionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Plan)
            .WithMany(x => x.PaymentTransactions)
            .HasForeignKey(x => x.PlanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.OwnerId);

        builder.HasIndex(x => x.OwnerSubscriptionId);

        builder.HasIndex(x => x.PlanId);

        builder.HasIndex(x => x.Status);

        builder.HasIndex(x => x.Provider);

        builder.HasIndex(x => x.PaymentMethod);

        builder.HasIndex(x => x.DueDate);

        builder.HasIndex(x => x.PaidAt);

        builder.HasIndex(x => x.ExternalPaymentId);

        builder.HasIndex(x => x.ExternalInvoiceId);

        builder.HasIndex(x => new
        {
            x.OwnerId,
            x.Status
        });

        builder.HasIndex(x => new
        {
            x.OwnerId,
            x.DueDate
        });
    }
}
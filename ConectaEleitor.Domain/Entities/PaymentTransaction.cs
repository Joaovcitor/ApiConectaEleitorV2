using ConectaEleitor.Domain.Enums;

namespace ConectaEleitor.Domain.Entities;

public class PaymentTransaction
{
    public Guid PaymentTransactionId { get; set; }

    public Guid OwnerId { get; set; }

    public Guid OwnerSubscriptionId { get; set; }
    public OwnerSubscription OwnerSubscription { get; set; } = null!;

    public Guid PlanId { get; set; }
    public Plan Plan { get; set; } = null!;

    public decimal Amount { get; set; }

    public string Currency { get; set; } = "BRL";

    public PaymentProvider Provider { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    public BillingCycle BillingCycle { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime? PaidAt { get; set; }

    public DateTime? FailedAt { get; set; }

    public DateTime? RefundedAt { get; set; }

    public string? ExternalPaymentId { get; set; }

    public string? ExternalInvoiceId { get; set; }

    public string? CheckoutUrl { get; set; }

    public string? PixQrCode { get; set; }

    public string? PixQrCodeBase64 { get; set; }

    public string? BoletoUrl { get; set; }

    public string? FailureReason { get; set; }

    public string? MetadataJson { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
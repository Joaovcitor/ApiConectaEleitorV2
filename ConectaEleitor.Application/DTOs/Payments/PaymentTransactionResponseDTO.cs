using ConectaEleitor.Application.DTOs.Plans;
using ConectaEleitor.Domain.Enums;

namespace ConectaEleitor.Application.DTOs.Payments;

public class PaymentTransactionResponseDTO
{
    public Guid PaymentTransactionId { get; set; }
    public Guid OwnerId { get; set; }
    public Guid OwnerSubscriptionId { get; set; }
    public Guid PlanId { get; set; }
    public PlanResponseDTO? Plan { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "BRL";
    public PaymentProvider Provider { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus Status { get; set; }
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
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

using ConectaEleitor.Domain.Enums;

namespace ConectaEleitor.Application.DTOs.Payments;

public class PaymentTransactionCreateDTO
{
    public Guid OwnerSubscriptionId { get; set; }
    public decimal Amount { get; set; }
    public string? Currency { get; set; }
    public PaymentProvider Provider { get; set; } = PaymentProvider.Manual;
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Manual;
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public BillingCycle BillingCycle { get; set; } = BillingCycle.Monthly;
    public DateTime DueDate { get; set; }
    public string? ExternalPaymentId { get; set; }
    public string? ExternalInvoiceId { get; set; }
    public string? CheckoutUrl { get; set; }
    public string? PixQrCode { get; set; }
    public string? PixQrCodeBase64 { get; set; }
    public string? BoletoUrl { get; set; }
    public string? MetadataJson { get; set; }
}

using ConectaEleitor.Domain.Enums;

namespace ConectaEleitor.Application.DTOs.Payments;

public class PaymentTransactionFilterDTO
{
    public PaymentStatus? Status { get; set; }
    public PaymentProvider? Provider { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid? SubscriptionId { get; set; }
}

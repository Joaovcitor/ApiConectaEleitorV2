using ConectaEleitor.Domain.Enums;

namespace ConectaEleitor.Application.DTOs.Subscriptions;

public class OwnerSubscriptionUpdateStatusDTO
{
    public SubscriptionStatus Status { get; set; }
    public string? Reason { get; set; }
}

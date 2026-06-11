using ConectaEleitor.Domain.Enums;

namespace ConectaEleitor.Application.DTOs.Subscriptions;

public class AdminChangeOwnerPlanDTO
{
    public Guid OwnerId { get; set; }
    public Guid NewPlanId { get; set; }
    public BillingCycle BillingCycle { get; set; } = BillingCycle.Monthly;
    public string? Reason { get; set; }
}

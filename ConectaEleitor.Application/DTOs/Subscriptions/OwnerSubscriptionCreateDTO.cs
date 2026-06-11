using ConectaEleitor.Domain.Enums;

namespace ConectaEleitor.Application.DTOs.Subscriptions;

public class OwnerSubscriptionCreateDTO
{
    public Guid PlanId { get; set; }
    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;
    public BillingCycle BillingCycle { get; set; } = BillingCycle.Monthly;
    public DateTime? CurrentPeriodStart { get; set; }
    public DateTime? CurrentPeriodEnd { get; set; }
    public DateTime? TrialEndsAt { get; set; }
}

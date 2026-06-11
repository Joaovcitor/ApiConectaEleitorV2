using ConectaEleitor.Domain.Enums;

namespace ConectaEleitor.Application.DTOs.Subscriptions;

public class ManualAssignPlanDTO
{
    public Guid OwnerId { get; set; }
    public Guid PlanId { get; set; }
    public BillingCycle BillingCycle { get; set; } = BillingCycle.Monthly;
    public DateTime? CurrentPeriodStart { get; set; }
    public DateTime? CurrentPeriodEnd { get; set; }
    public DateTime? TrialEndsAt { get; set; }
    public string? Notes { get; set; }
}

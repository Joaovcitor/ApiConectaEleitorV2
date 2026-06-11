using ConectaEleitor.Application.DTOs.Plans;
using ConectaEleitor.Domain.Enums;

namespace ConectaEleitor.Application.DTOs.Subscriptions;

public class OwnerSubscriptionResponseDTO
{
    public Guid OwnerSubscriptionId { get; set; }
    public Guid OwnerId { get; set; }
    public Guid PlanId { get; set; }
    public string? PlanName { get; set; }
    public string? PlanSlug { get; set; }
    public PlanResponseDTO? Plan { get; set; }
    public SubscriptionStatus Status { get; set; }
    public BillingCycle BillingCycle { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CurrentPeriodStart { get; set; }
    public DateTime? CurrentPeriodEnd { get; set; }
    public DateTime? TrialEndsAt { get; set; }
    public DateTime? CanceledAt { get; set; }
    public DateTime? SuspendedAt { get; set; }
    public string? CancelReason { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public IEnumerable<SubscriptionHistoryResponseDTO> History { get; set; } = [];
}

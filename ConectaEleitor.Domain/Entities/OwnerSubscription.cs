using ConectaEleitor.Domain.Enums;

namespace ConectaEleitor.Domain.Entities;

public class OwnerSubscription
{
    public Guid OwnerSubscriptionId { get; set; }

    public Guid OwnerId { get; set; }

    public Guid PlanId { get; set; }
    public Plan Plan { get; set; } = null!;

    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;

    public BillingCycle BillingCycle { get; set; } = BillingCycle.Monthly;

    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CurrentPeriodStart { get; set; }
    public DateTime? CurrentPeriodEnd { get; set; }

    public DateTime? TrialEndsAt { get; set; }
    public DateTime? CanceledAt { get; set; }
    public DateTime? SuspendedAt { get; set; }

    public string? CancelReason { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public ICollection<SubscriptionHistory> History { get; set; } = [];
    public ICollection<PaymentTransaction> Payments { get; set; } = [];
}
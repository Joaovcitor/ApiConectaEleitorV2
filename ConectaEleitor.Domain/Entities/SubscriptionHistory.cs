namespace ConectaEleitor.Domain.Entities;
public class SubscriptionHistory
{
    public Guid SubscriptionHistoryId { get; set; }

    public Guid OwnerSubscriptionId { get; set; }
    public OwnerSubscription OwnerSubscription { get; set; } = null!;

    public SubscriptionAction Action { get; set; }

    public Guid? OldPlanId { get; set; }
    public Guid? NewPlanId { get; set; }

    public string? Description { get; set; }

    public Guid? ChangedByUserId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
using ConectaEleitor.Domain.Entities;

namespace ConectaEleitor.Application.DTOs.Subscriptions;

public class SubscriptionHistoryResponseDTO
{
    public Guid SubscriptionHistoryId { get; set; }
    public Guid OwnerSubscriptionId { get; set; }
    public SubscriptionAction Action { get; set; }
    public Guid? OldPlanId { get; set; }
    public Guid? NewPlanId { get; set; }
    public string? Description { get; set; }
    public Guid? ChangedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }
}

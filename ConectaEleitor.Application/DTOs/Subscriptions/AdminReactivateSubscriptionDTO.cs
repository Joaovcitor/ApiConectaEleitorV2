namespace ConectaEleitor.Application.DTOs.Subscriptions;

public class AdminReactivateSubscriptionDTO
{
    public Guid OwnerId { get; set; }
    public string? Reason { get; set; }
}

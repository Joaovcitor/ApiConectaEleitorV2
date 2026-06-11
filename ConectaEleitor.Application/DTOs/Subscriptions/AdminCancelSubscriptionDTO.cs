namespace ConectaEleitor.Application.DTOs.Subscriptions;

public class AdminCancelSubscriptionDTO
{
    public Guid OwnerId { get; set; }
    public string? Reason { get; set; }
}

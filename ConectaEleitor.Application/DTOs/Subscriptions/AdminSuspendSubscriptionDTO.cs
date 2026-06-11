namespace ConectaEleitor.Application.DTOs.Subscriptions;

public class AdminSuspendSubscriptionDTO
{
    public Guid OwnerId { get; set; }
    public string? Reason { get; set; }
}

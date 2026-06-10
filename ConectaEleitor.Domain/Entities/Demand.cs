using ConectaEleitor.Domain.Enums;

namespace ConectaEleitor.Domain.Entities;

public class Demand
{
    public Guid DemandId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public DemandStatus Status { get; set; }

    public DemandPriority Priority { get; set; }

    public Guid CitizenId { get; set; }
    public Citizen Citizen { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }

    public Guid OwnerId { get; set; }
}
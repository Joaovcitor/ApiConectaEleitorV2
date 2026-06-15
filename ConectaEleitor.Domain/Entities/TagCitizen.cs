namespace ConectaEleitor.Domain.Entities;

public class TagCitizen
{
    public Guid TagId { get; set; }

    public Guid CitizenId { get; set; }

    public string? Reason { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Tag Tag { get; set; } = null!;
    public Citizen Citizen { get; set; } = null!;
}
namespace ConectaEleitor.Domain.Entities;

public class Tag
{
    public Guid TagId { get; set; }
    public Guid OwnerId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Color { get; set; } = "#3B82F6";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<TagCitizen> TagsCitizens { get; set; } = [];
}
namespace ConectaEleitor.Domain.Entities;

public class PlanFeature
{
    public Guid PlanFeatureId { get; set; }

    public Guid PlanId { get; set; }
    public Plan Plan { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsEnabled { get; set; } = true;
}
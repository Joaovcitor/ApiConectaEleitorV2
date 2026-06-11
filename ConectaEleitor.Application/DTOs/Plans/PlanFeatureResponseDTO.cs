namespace ConectaEleitor.Application.DTOs.Plans;

public class PlanFeatureResponseDTO
{
    public Guid PlanFeatureId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsEnabled { get; set; }
}

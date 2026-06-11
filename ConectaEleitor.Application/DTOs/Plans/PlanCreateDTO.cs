namespace ConectaEleitor.Application.DTOs.Plans;

public class PlanCreateDTO
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal MonthlyPrice { get; set; }
    public decimal? YearlyPrice { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsPublic { get; set; } = true;
    public int DisplayOrder { get; set; }
    public List<PlanFeatureCreateDTO> Features { get; set; } = [];
    public List<PlanLimitCreateDTO> Limits { get; set; } = [];
}

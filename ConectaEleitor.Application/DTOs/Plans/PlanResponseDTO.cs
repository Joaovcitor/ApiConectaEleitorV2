namespace ConectaEleitor.Application.DTOs.Plans;

public class PlanResponseDTO
{
    public Guid PlanId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal MonthlyPrice { get; set; }
    public decimal? YearlyPrice { get; set; }
    public bool IsActive { get; set; }
    public bool IsPublic { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public IEnumerable<PlanFeatureResponseDTO> Features { get; set; } = [];
    public IEnumerable<PlanLimitResponseDTO> Limits { get; set; } = [];
}

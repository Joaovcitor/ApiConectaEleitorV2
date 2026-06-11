using ConectaEleitor.Domain.Enums;

namespace ConectaEleitor.Application.DTOs.Plans;

public class PlanLimitResponseDTO
{
    public Guid PlanLimitId { get; set; }
    public PlanLimitType Type { get; set; }
    public int? Value { get; set; }
    public bool IsUnlimited { get; set; }
}

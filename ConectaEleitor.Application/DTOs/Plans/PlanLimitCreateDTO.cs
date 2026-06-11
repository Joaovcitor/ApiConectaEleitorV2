using ConectaEleitor.Domain.Enums;

namespace ConectaEleitor.Application.DTOs.Plans;

public class PlanLimitCreateDTO
{
    public PlanLimitType Type { get; set; }
    public int? Value { get; set; }
    public bool IsUnlimited { get; set; }
}

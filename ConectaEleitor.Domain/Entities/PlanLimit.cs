using ConectaEleitor.Domain.Enums;

namespace ConectaEleitor.Domain.Entities;

public class PlanLimit
{
    public Guid PlanLimitId { get; set; }

    public Guid PlanId { get; set; }
    public Plan Plan { get; set; } = null!;

    public PlanLimitType Type { get; set; }

    public int? Value { get; set; }

    public bool IsUnlimited { get; set; } = false;
}
namespace ConectaEleitor.Domain.Entities;

public class Plan
{
    public Guid PlanId { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal MonthlyPrice { get; set; }
    public decimal? YearlyPrice { get; set; }

    public bool IsActive { get; set; } = true;
    public bool IsPublic { get; set; } = true;

    public int DisplayOrder { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public ICollection<PlanFeature> Features { get; set; } = [];
    public ICollection<PlanLimit> Limits { get; set; } = [];
    public ICollection<OwnerSubscription> Subscriptions { get; set; } = [];
    public ICollection<PaymentTransaction> PaymentTransactions { get; set; } = [];
}
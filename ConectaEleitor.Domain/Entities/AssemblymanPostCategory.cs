namespace ConectaEleitor.Domain.Entities;

public class AssemblymanPostCategory
{
    public Guid AssemblymanPostCategoryId { get; set; }

    public Guid OwnerId { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ICollection<AssemblymanPost> Posts { get; set; } = [];
}
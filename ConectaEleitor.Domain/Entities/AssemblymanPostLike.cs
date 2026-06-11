namespace ConectaEleitor.Domain.Entities;

public class AssemblymanPostLike
{
    public Guid AssemblymanPostLikeId { get; set; }

    public Guid PostId { get; set; }
    public AssemblymanPost Post { get; set; }

    public Guid UserId { get; set; }

    public DateTime CreatedAt { get; set; }
}
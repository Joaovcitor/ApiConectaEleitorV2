namespace ConectaEleitor.Domain.Entities;

public class AssemblymanPostComment
{
    public Guid AssemblymanPostCommentId { get; set; }

    public Guid PostId { get; set; }
    public AssemblymanPost Post { get; set; }

    public Guid UserId { get; set; }

    public string Content { get; set; } = null!;

    // Permite respostas a comentários
    public Guid? ParentCommentId { get; set; }
    public AssemblymanPostComment? ParentComment { get; set; }

    public ICollection<AssemblymanPostComment> Replies { get; set; } = [];

    public DateTime CreatedAt { get; set; }
}
namespace ConectaEleitor.Application.DTOs.AssemblymanPostsDTOs;

public class AssemblymanPostCommentResponseDTO
{
    public Guid AssemblymanPostCommentId { get; set; }
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }

    public string Content { get; set; } = null!;

    public Guid? ParentCommentId { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<AssemblymanPostCommentResponseDTO> Replies { get; set; } = [];
}
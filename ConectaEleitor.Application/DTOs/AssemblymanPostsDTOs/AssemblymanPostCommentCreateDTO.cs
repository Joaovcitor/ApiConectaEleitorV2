namespace ConectaEleitor.Application.DTOs.AssemblymanPostsDTOs;

public class AssemblymanPostCommentCreateDTO
{
    public Guid PostId { get; set; }
    public string Content { get; set; } = null!;
    public Guid? ParentCommentId { get; set; }
}
namespace ConectaEleitor.Application.DTOs.AssemblymanPostsDTOs;

public class AssemblymanPostCreateDTO
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string? Summary { get; set; }
    public string? CoverImageUrl { get; set; }
    public bool IsPinned { get; set; }
    public bool IsPublished { get; set; } = true;
    public Guid? CategoryId { get; set; }
}
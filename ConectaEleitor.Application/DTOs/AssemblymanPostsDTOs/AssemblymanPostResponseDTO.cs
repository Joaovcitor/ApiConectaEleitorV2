namespace ConectaEleitor.Application.DTOs.AssemblymanPostsDTOs;

public class AssemblymanPostResponseDTO
{
    public Guid AssemblymanPostId { get; set; }
    public Guid OwnerId { get; set; }
    public Guid UserId { get; set; }

    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string? Summary { get; set; }
    public string? CoverImageUrl { get; set; }

    public bool IsPinned { get; set; }
    public bool IsPublished { get; set; }

    public int ViewsCount { get; set; }
    public int LikesCount { get; set; }
    public int CommentsCount { get; set; }

    public Guid? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
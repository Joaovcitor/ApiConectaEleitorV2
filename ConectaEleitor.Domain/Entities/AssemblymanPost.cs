namespace ConectaEleitor.Domain.Entities;

public class AssemblymanPost
{
    public Guid AssemblymanPostId { get; set; }

    public Guid OwnerId { get; set; }

    // Autor do post
    public Guid UserId { get; set; }
    
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;

    // Resumo opcional
    public string? Summary { get; set; }

    // Imagem de capa
    public string? CoverImageUrl { get; set; }

    // Visibilidade
    public bool IsPinned { get; set; }
    public bool IsPublished { get; set; } = true;

    // Métricas
    public int ViewsCount { get; set; }
    public int LikesCount { get; set; }
    public int CommentsCount { get; set; }
    
    // Categorias
    public Guid? CategoryId { get; set; }
    public AssemblymanPostCategory? Category { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ICollection<AssemblymanPostComment> Comments { get; set; } = [];
    public ICollection<AssemblymanPostLike> Likes { get; set; } = [];
}

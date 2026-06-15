namespace ConectaEleitor.Application.DTOs.TagsDTOs;

public class TagResponseDTO
{
    public Guid TagId { get; set; }
    public Guid OwnerId { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Color { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
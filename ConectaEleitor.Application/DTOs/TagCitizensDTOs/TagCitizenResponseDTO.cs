namespace ConectaEleitor.Application.DTOs.TagCitizensDTOs;

public class TagCitizenResponseDTO
{
    public Guid TagId { get; set; }
    public Guid CitizenId { get; set; }

    public string? Reason { get; set; }
    public DateTime CreatedAt { get; set; }

    public string TagName { get; set; } = null!;
    public string? TagDescription { get; set; }
    public string? TagColor { get; set; }

    public string CitizenName { get; set; } = null!;
}
namespace ConectaEleitor.Application.DTOs.PoliticalProfiles;

public class PoliticalProfileResponseDTO
{
    public Guid PoliticalProfileId { get; set; }

    public Guid OwnerId { get; set; }
    public Guid UserId { get; set; }

    public string DisplayName { get; set; } = null!;
    public string? Bio { get; set; }

    public string? PhotoUrl { get; set; }
    public string? BannerUrl { get; set; }

    public string? Office { get; set; }
    public string? Party { get; set; }

    public string? City { get; set; }
    public string? State { get; set; }

    public bool IsVerified { get; set; }
    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
using ConectaEleitor.Domain.Enums;

namespace ConectaEleitor.Application.DTOs.CitizenDTOs;

public class CitizenResponseDTO
{
    public Guid CitizenId { get; set; }

    public string FullName { get; set; } = string.Empty;
    public string? Nickname { get; set; }

    public string? Phone { get; set; }
    public string? WhatsApp { get; set; }

    public string? Neighborhood { get; set; }
    public string? District { get; set; }

    public string? ElectoralZone { get; set; }
    public string? ElectoralSection { get; set; }

    public CitizenType Type { get; set; }

    public Guid? LeaderId { get; set; }
    public string? LeaderName { get; set; }

    public Guid? UserId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}
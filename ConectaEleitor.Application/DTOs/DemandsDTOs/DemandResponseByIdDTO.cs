using ConectaEleitor.Domain.Enums;

namespace ConectaEleitor.Application.DTOs.DemandsDTOs;

public class DemandResponseByIdDTO
{
    public Guid DemandId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public DemandStatus Status { get; set; }
    public DemandPriority Priority { get; set; }

    public Guid CitizenId { get; set; }
    public string CitizenName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
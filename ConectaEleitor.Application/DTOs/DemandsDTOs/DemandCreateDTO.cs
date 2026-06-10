using ConectaEleitor.Domain.Enums;

namespace ConectaEleitor.Application.DTOs.DemandsDTOs;

public class DemandCreateDTO
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public DemandPriority Priority { get; set; }

    public Guid CitizenId { get; set; }
}
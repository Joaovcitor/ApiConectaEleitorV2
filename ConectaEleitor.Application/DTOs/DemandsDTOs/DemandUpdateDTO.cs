using ConectaEleitor.Domain.Enums;

namespace ConectaEleitor.Application.DTOs.DemandsDTOs;

public class DemandUpdateDTO
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public DemandStatus Status { get; set; }
    public DemandPriority Priority { get; set; }

    public Guid CitizenId { get; set; }
}
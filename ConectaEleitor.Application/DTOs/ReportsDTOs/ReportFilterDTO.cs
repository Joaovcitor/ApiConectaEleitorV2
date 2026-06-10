using ConectaEleitor.Domain.Enums;

namespace ConectaEleitor.Application.DTOs.ReportsDTOs;

public class ReportFilterDTO
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public string? Neighborhood { get; set; }
    public string? District { get; set; }

    public CitizenType? CitizenType { get; set; }
    public DemandStatus? DemandStatus { get; set; }

    public Guid? LeaderId { get; set; }
    public Guid? CitizenId { get; set; }
}
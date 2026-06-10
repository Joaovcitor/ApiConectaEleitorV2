using ConectaEleitor.Domain.Enums;

namespace ConectaEleitor.Application.DTOs.ReportsDTOs;

public class ReportBaseFilterDTO
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public string? Neighborhood { get; set; }
    public string? District { get; set; }

    public Guid? CitizenId { get; set; }
    public Guid? LeaderId { get; set; }
}
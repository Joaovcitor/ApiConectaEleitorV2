using ConectaEleitor.Domain.Enums;

namespace ConectaEleitor.Application.DTOs.ReportsDTOs;

public class DemandReportFilterDTO : ReportBaseFilterDTO
{
    public DemandStatus? Status { get; set; }
}
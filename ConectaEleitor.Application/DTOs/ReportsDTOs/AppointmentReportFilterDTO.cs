using ConectaEleitor.Domain.Enums;

namespace ConectaEleitor.Application.DTOs.ReportsDTOs;

public class AppointmentReportFilterDTO : ReportBaseFilterDTO
{
    public AppointmentStatus? Status { get; set; }
    public Guid? ResponsibleUserId { get; set; }
}
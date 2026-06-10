using ConectaEleitor.Application.DTOs.ReportsDTOs;

namespace ConectaEleitor.Application.Interfaces;

public interface IReportService
{
    Task<DashboardReportDTO> GetDashboardAsync(ReportBaseFilterDTO filter);

    Task<DemandReportDTO> GetDemandsReportAsync(DemandReportFilterDTO filter);

    Task<AppointmentReportDTO> GetAppointmentsReportAsync(AppointmentReportFilterDTO filter);

    Task<List<LeaderRankingDTO>> GetTopLeadersAsync(ReportBaseFilterDTO filter);
}
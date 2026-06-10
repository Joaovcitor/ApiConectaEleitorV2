using ConectaEleitor.Application.DTOs.ReportsDTOs;

namespace ConectaEleitor.Application.Interfaces;

public interface IReportRepository
{
    Task<DashboardReportDTO> GetDashboardAsync(
        Guid ownerId,
        ReportBaseFilterDTO filter);

    Task<DemandReportDTO> GetDemandsReportAsync(
        Guid ownerId,
        DemandReportFilterDTO filter);

    Task<AppointmentReportDTO> GetAppointmentsReportAsync(
        Guid ownerId,
        AppointmentReportFilterDTO filter);

    Task<List<LeaderRankingDTO>> GetTopLeadersAsync(
        Guid ownerId,
        ReportBaseFilterDTO filter);
}
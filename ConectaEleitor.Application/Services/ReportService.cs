using ConectaEleitor.Application.DTOs.ReportsDTOs;
using ConectaEleitor.Application.Interfaces;

namespace ConectaEleitor.Application.Services;

public class ReportService : IReportService
{
    private readonly IReportRepository _reportRepository;
    private readonly IUserContext _userContext;

    public ReportService(
        IReportRepository reportRepository,
        IUserContext userContext)
    {
        _reportRepository = reportRepository;
        _userContext = userContext;
    }

    public async Task<DashboardReportDTO> GetDashboardAsync(
        ReportBaseFilterDTO filter)
    {
        var ownerId = _userContext.OwnerId;

        return await _reportRepository.GetDashboardAsync(ownerId, filter);
    }

    public async Task<DemandReportDTO> GetDemandsReportAsync(
        DemandReportFilterDTO filter)
    {
        var ownerId = _userContext.OwnerId;

        return await _reportRepository.GetDemandsReportAsync(ownerId, filter);
    }

    public async Task<AppointmentReportDTO> GetAppointmentsReportAsync(
        AppointmentReportFilterDTO filter)
    {
        var ownerId = _userContext.OwnerId;

        return await _reportRepository.GetAppointmentsReportAsync(ownerId, filter);
    }

    public async Task<List<LeaderRankingDTO>> GetTopLeadersAsync(
        ReportBaseFilterDTO filter)
    {
        var ownerId = _userContext.OwnerId;

        return await _reportRepository.GetTopLeadersAsync(ownerId, filter);
    }
}
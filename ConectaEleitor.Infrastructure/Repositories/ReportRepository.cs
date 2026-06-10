using ConectaEleitor.Application.DTOs.ReportsDTOs;
using ConectaEleitor.Application.Interfaces;
using ConectaEleitor.Domain.Entities;
using ConectaEleitor.Domain.Enums;
using ConectaEleitor.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ConectaEleitor.Infrastructure.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly AppDbContext _context;

    public ReportRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardReportDTO> GetDashboardAsync(
        Guid ownerId,
        ReportBaseFilterDTO filter)
    {
        var citizensQuery = ApplyCitizenFilters(
            _context.Citizens
                .AsNoTracking()
                .Where(c => c.OwnerId == ownerId),
            filter);

        var demandFilter = new DemandReportFilterDTO
        {
            StartDate = filter.StartDate,
            EndDate = filter.EndDate,
            Neighborhood = filter.Neighborhood,
            District = filter.District,
            CitizenId = filter.CitizenId,
            LeaderId = filter.LeaderId
        };

        var appointmentFilter = new AppointmentReportFilterDTO
        {
            StartDate = filter.StartDate,
            EndDate = filter.EndDate,
            Neighborhood = filter.Neighborhood,
            District = filter.District,
            CitizenId = filter.CitizenId,
            LeaderId = filter.LeaderId
        };

        return new DashboardReportDTO
        {
            TotalCitizens = await citizensQuery.CountAsync(),

            TotalActiveCitizens = await citizensQuery
                .CountAsync(c => c.IsActive),

            TotalInactiveCitizens = await citizensQuery
                .CountAsync(c => !c.IsActive),

            TotalLeaders = await citizensQuery
                .CountAsync(c => c.Type == CitizenType.Leader),

            TotalVoters = await citizensQuery
                .CountAsync(c => c.Type == CitizenType.Voter),

            CitizensByType = await citizensQuery
                .GroupBy(c => c.Type)
                .Select(g => new CitizensByTypeDTO
                {
                    Type = g.Key,
                    Total = g.Count()
                })
                .ToListAsync(),

            CitizensByNeighborhood = await citizensQuery
                .Where(c => c.Neighborhood != null && c.Neighborhood != "")
                .GroupBy(c => c.Neighborhood!)
                .Select(g => new CitizensByNeighborhoodDTO
                {
                    Neighborhood = g.Key,
                    Total = g.Count()
                })
                .OrderByDescending(x => x.Total)
                .ToListAsync(),

            CitizensByDistrict = await citizensQuery
                .Where(c => c.District != null && c.District != "")
                .GroupBy(c => c.District!)
                .Select(g => new CitizensByDistrictDTO
                {
                    District = g.Key,
                    Total = g.Count()
                })
                .OrderByDescending(x => x.Total)
                .ToListAsync(),

            Demands = await GetDemandsReportAsync(ownerId, demandFilter),

            Appointments = await GetAppointmentsReportAsync(ownerId, appointmentFilter),

            TopLeaders = await GetTopLeadersAsync(ownerId, filter)
        };
    }

    public async Task<DemandReportDTO> GetDemandsReportAsync(
        Guid ownerId,
        DemandReportFilterDTO filter)
    {
        var query = ApplyDemandFilters(
            _context.Demands
                .AsNoTracking()
                .Where(d => d.OwnerId == ownerId),
            filter);

        return new DemandReportDTO
        {
            TotalDemands = await query.CountAsync(),

            OpenDemands = await query
                .CountAsync(d => d.Status == DemandStatus.Open),

            InProgressDemands = await query
                .CountAsync(d => d.Status == DemandStatus.InProgress),

            CompletedDemands = await query
                .CountAsync(d => d.Status == DemandStatus.Resolved),

            CanceledDemands = await query
                .CountAsync(d => d.Status == DemandStatus.Canceled),

            DemandsByStatus = await query
                .GroupBy(d => d.Status)
                .Select(g => new DemandsByStatusDTO
                {
                    Status = g.Key,
                    Total = g.Count()
                })
                .ToListAsync(),

            DemandsByMonth = await query
                .GroupBy(d => new
                {
                    d.CreatedAt.Year,
                    d.CreatedAt.Month
                })
                .Select(g => new DemandsByMonthDTO
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Total = g.Count()
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync()
        };
    }

    public async Task<AppointmentReportDTO> GetAppointmentsReportAsync(
        Guid ownerId,
        AppointmentReportFilterDTO filter)
    {
        var query = ApplyAppointmentFilters(
            _context.Appointments
                .AsNoTracking()
                .Where(a => a.OwnerId == ownerId),
            filter);

        return new AppointmentReportDTO
        {
            TotalAppointments = await query.CountAsync(),

            ScheduledAppointments = await query
                .CountAsync(a => a.Status == AppointmentStatus.Scheduled),

            CompletedAppointments = await query
                .CountAsync(a => a.Status == AppointmentStatus.Completed),

            CanceledAppointments = await query
                .CountAsync(a => a.Status == AppointmentStatus.Canceled),

            AppointmentsByStatus = await query
                .GroupBy(a => a.Status)
                .Select(g => new AppointmentsByStatusDTO
                {
                    Status = g.Key,
                    Total = g.Count()
                })
                .ToListAsync(),

            AppointmentsByMonth = await query
                .GroupBy(a => new
                {
                    a.StartAt.Year,
                    a.StartAt.Month
                })
                .Select(g => new AppointmentsByMonthDTO
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Total = g.Count()
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync()
        };
    }

    public async Task<List<LeaderRankingDTO>> GetTopLeadersAsync(
        Guid ownerId,
        ReportBaseFilterDTO filter)
    {
        var ledCitizensQuery = ApplyCitizenFilters(
            _context.Citizens
                .AsNoTracking()
                .Where(c =>
                    c.OwnerId == ownerId &&
                    c.LeaderId != null),
            filter);

        return await _context.Citizens
            .AsNoTracking()
            .Where(l =>
                l.OwnerId == ownerId &&
                l.Type == CitizenType.Leader)
            .Select(leader => new LeaderRankingDTO
            {
                LeaderId = leader.CitizenId,
                LeaderName = leader.FullName,
                TotalLedCitizens = ledCitizensQuery.Count(c =>
                    c.LeaderId == leader.CitizenId)
            })
            .Where(x => x.TotalLedCitizens > 0)
            .OrderByDescending(x => x.TotalLedCitizens)
            .Take(10)
            .ToListAsync();
    }

    private static IQueryable<Citizen> ApplyCitizenFilters(
        IQueryable<Citizen> query,
        ReportBaseFilterDTO filter)
    {
        var startDate = ToUtcStart(filter.StartDate);
        var endDate = ToUtcEnd(filter.EndDate);

        if (startDate.HasValue)
            query = query.Where(c => c.CreatedAt >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(c => c.CreatedAt <= endDate.Value);

        if (!string.IsNullOrWhiteSpace(filter.Neighborhood))
            query = query.Where(c => c.Neighborhood == filter.Neighborhood);

        if (!string.IsNullOrWhiteSpace(filter.District))
            query = query.Where(c => c.District == filter.District);

        if (filter.CitizenId.HasValue)
            query = query.Where(c => c.CitizenId == filter.CitizenId.Value);

        if (filter.LeaderId.HasValue)
            query = query.Where(c => c.LeaderId == filter.LeaderId.Value);

        return query;
    }

    private static IQueryable<Demand> ApplyDemandFilters(
        IQueryable<Demand> query,
        DemandReportFilterDTO filter)
    {
        var startDate = ToUtcStart(filter.StartDate);
        var endDate = ToUtcEnd(filter.EndDate);

        if (startDate.HasValue)
            query = query.Where(d => d.CreatedAt >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(d => d.CreatedAt <= endDate.Value);

        if (filter.Status.HasValue)
            query = query.Where(d => d.Status == filter.Status.Value);

        if (filter.CitizenId.HasValue)
            query = query.Where(d => d.CitizenId == filter.CitizenId.Value);

        if (filter.LeaderId.HasValue)
            query = query.Where(d => d.Citizen.LeaderId == filter.LeaderId.Value);

        if (!string.IsNullOrWhiteSpace(filter.Neighborhood))
            query = query.Where(d => d.Citizen.Neighborhood == filter.Neighborhood);

        if (!string.IsNullOrWhiteSpace(filter.District))
            query = query.Where(d => d.Citizen.District == filter.District);

        return query;
    }

    private static IQueryable<Appointment> ApplyAppointmentFilters(
        IQueryable<Appointment> query,
        AppointmentReportFilterDTO filter)
    {
        var startDate = ToUtcStart(filter.StartDate);
        var endDate = ToUtcEnd(filter.EndDate);

        if (startDate.HasValue)
            query = query.Where(a => a.StartAt >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(a => a.StartAt <= endDate.Value);

        if (filter.Status.HasValue)
            query = query.Where(a => a.Status == filter.Status.Value);

        if (filter.CitizenId.HasValue)
            query = query.Where(a => a.CitizenId == filter.CitizenId.Value);

        if (filter.LeaderId.HasValue)
            query = query.Where(a => a.Citizen.LeaderId == filter.LeaderId.Value);

        if (!string.IsNullOrWhiteSpace(filter.Neighborhood))
            query = query.Where(a => a.Citizen.Neighborhood == filter.Neighborhood);

        if (!string.IsNullOrWhiteSpace(filter.District))
            query = query.Where(a => a.Citizen.District == filter.District);

        return query;
    }

    private static DateTime? ToUtcStart(DateTime? date)
    {
        if (!date.HasValue)
            return null;

        return DateTime.SpecifyKind(date.Value.Date, DateTimeKind.Utc);
    }

    private static DateTime? ToUtcEnd(DateTime? date)
    {
        if (!date.HasValue)
            return null;

        var endOfDay = date.Value.Date
            .AddDays(1)
            .AddTicks(-1);

        return DateTime.SpecifyKind(endOfDay, DateTimeKind.Utc);
    }
}
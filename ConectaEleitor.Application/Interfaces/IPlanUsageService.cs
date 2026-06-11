using ConectaEleitor.Application.DTOs.PlanUsages;
using ConectaEleitor.Domain.Enums;

namespace ConectaEleitor.Application.Interfaces;

public interface IPlanUsageService
{
    Task<PlanUsageResponseDTO> GetCurrentUsageAsync();
    Task<PlanUsageResponseDTO> GetByPeriodAsync(int year, int month);
    Task<PlanUsageResponseDTO> UpdateUsageAsync(PlanUsageUpdateDTO dto);
    Task EnsureCanUseAsync(PlanLimitType limitType);
    Task EnsureCanCreateVoterAsync();
    Task EnsureCanCreateLeaderAsync();
    Task EnsureCanCreateDemandAsync();
    Task EnsureCanCreateAppointmentAsync();
}

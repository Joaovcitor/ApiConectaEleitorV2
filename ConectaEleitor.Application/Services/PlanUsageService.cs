using AutoMapper;
using ConectaEleitor.Application.DTOs.PlanUsages;
using ConectaEleitor.Application.Interfaces;
using ConectaEleitor.Domain.Entities;
using ConectaEleitor.Domain.Enums;
using ConectaEleitor.Domain.Exceptions;

namespace ConectaEleitor.Application.Services;

public class PlanUsageService : IPlanUsageService
{
    private readonly IPlanUsageRepository _usageRepository;
    private readonly IOwnerSubscriptionRepository _subscriptionRepository;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;

    public PlanUsageService(
        IPlanUsageRepository usageRepository,
        IOwnerSubscriptionRepository subscriptionRepository,
        IUserContext userContext,
        IMapper mapper)
    {
        _usageRepository = usageRepository;
        _subscriptionRepository = subscriptionRepository;
        _userContext = userContext;
        _mapper = mapper;
    }

    public async Task<PlanUsageResponseDTO> GetCurrentUsageAsync()
    {
        var now = DateTime.UtcNow;
        return _mapper.Map<PlanUsageResponseDTO>(await GetOrCreateUsageAsync(_userContext.OwnerId, now.Year, now.Month));
    }

    public async Task<PlanUsageResponseDTO> GetByPeriodAsync(int year, int month)
    {
        ValidatePeriod(year, month);
        return _mapper.Map<PlanUsageResponseDTO>(await GetOrCreateUsageAsync(_userContext.OwnerId, year, month));
    }

    public async Task<PlanUsageResponseDTO> UpdateUsageAsync(PlanUsageUpdateDTO dto)
    {
        var now = DateTime.UtcNow;
        var usage = await GetOrCreateUsageAsync(_userContext.OwnerId, now.Year, now.Month);

        _mapper.Map(dto, usage);
        usage.UpdatedAt = DateTime.UtcNow;

        _usageRepository.Update(usage);
        await _usageRepository.SaveChangesAsync();

        return _mapper.Map<PlanUsageResponseDTO>(usage);
    }

    public async Task EnsureCanUseAsync(PlanLimitType limitType)
    {
        var subscription = await _subscriptionRepository.GetActiveByOwnerIdAsync(_userContext.OwnerId);
        if (subscription is null)
            throw new BadRequestException("Nenhuma assinatura ativa encontrada.");

        var limit = subscription.Plan.Limits.FirstOrDefault(x => x.Type == limitType);
        if (limit is null)
            throw new BadRequestException("Limite do plano não configurado.");

        if (limit.IsUnlimited)
            return;

        if (!limit.Value.HasValue)
            throw new BadRequestException("Este recurso não está disponível no plano atual.");

        var now = DateTime.UtcNow;
        var usage = await GetOrCreateUsageAsync(_userContext.OwnerId, now.Year, now.Month);
        var currentValue = GetUsageValue(usage, limitType);

        if (currentValue >= limit.Value.Value)
            throw new BadRequestException("Limite do plano excedido para este recurso.");
    }

    public Task EnsureCanCreateVoterAsync() => EnsureCanUseAsync(PlanLimitType.Voters);

    public Task EnsureCanCreateLeaderAsync() => EnsureCanUseAsync(PlanLimitType.Leaders);

    public Task EnsureCanCreateDemandAsync() => EnsureCanUseAsync(PlanLimitType.Demands);

    public Task EnsureCanCreateAppointmentAsync() => EnsureCanUseAsync(PlanLimitType.Appointments);

    private async Task<PlanUsage> GetOrCreateUsageAsync(Guid ownerId, int year, int month)
    {
        var usage = await _usageRepository.GetByOwnerAndPeriodAsync(ownerId, year, month);
        if (usage is not null)
            return usage;

        usage = new PlanUsage
        {
            PlanUsageId = Guid.NewGuid(),
            OwnerId = ownerId,
            Year = year,
            Month = month,
            UpdatedAt = DateTime.UtcNow
        };

        await _usageRepository.AddAsync(usage);
        await _usageRepository.SaveChangesAsync();
        return usage;
    }

    private static int GetUsageValue(PlanUsage usage, PlanLimitType limitType)
    {
        return limitType switch
        {
            PlanLimitType.Users => usage.UsersCount,
            PlanLimitType.Voters => usage.VotersCount,
            PlanLimitType.Leaders => usage.LeadersCount,
            PlanLimitType.Demands => usage.DemandsCount,
            PlanLimitType.Appointments => usage.AppointmentsCount,
            PlanLimitType.Reports => usage.ReportsGeneratedCount,
            PlanLimitType.Exports => usage.ExportsGeneratedCount,
            PlanLimitType.FileStorageMb => (int)(usage.StorageUsedBytes / 1024 / 1024),
            _ => 0
        };
    }

    private static void ValidatePeriod(int year, int month)
    {
        if (year < 2000 || month is < 1 or > 12)
            throw new BadRequestException("Período inválido.");
    }
}

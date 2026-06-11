using AutoMapper;
using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.DTOs.Subscriptions;
using ConectaEleitor.Application.Interfaces;
using ConectaEleitor.Domain.Entities;
using ConectaEleitor.Domain.Enums;
using ConectaEleitor.Domain.Exceptions;

namespace ConectaEleitor.Application.Services;

public class OwnerSubscriptionService : IOwnerSubscriptionService
{
    private readonly IOwnerSubscriptionRepository _subscriptionRepository;
    private readonly IPlanRepository _planRepository;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;

    public OwnerSubscriptionService(
        IOwnerSubscriptionRepository subscriptionRepository,
        IPlanRepository planRepository,
        IUserContext userContext,
        IMapper mapper)
    {
        _subscriptionRepository = subscriptionRepository;
        _planRepository = planRepository;
        _userContext = userContext;
        _mapper = mapper;
    }

    public async Task<OwnerSubscriptionResponseDTO> CreateAsync(OwnerSubscriptionCreateDTO dto)
    {
        EnsureAdmin();

        var ownerId = _userContext.OwnerId;
        var plan = await _planRepository.GetByIdAsync(dto.PlanId);

        if (plan is null)
            throw new NotFoundException("Plano não encontrado.");

        if (!plan.IsActive)
            throw new BadRequestException("Não é possível assinar um plano inativo.");

        if (await _subscriptionRepository.HasActiveOrTrialByOwnerIdAsync(ownerId))
            throw new BadRequestException("Já existe uma assinatura ativa ou trial para este tenant.");

        var now = DateTime.UtcNow;
        var subscription = new OwnerSubscription
        {
            OwnerSubscriptionId = Guid.NewGuid(),
            OwnerId = ownerId,
            PlanId = dto.PlanId,
            Status = dto.Status,
            BillingCycle = dto.BillingCycle,
            StartedAt = now,
            CurrentPeriodStart = dto.CurrentPeriodStart ?? now,
            CurrentPeriodEnd = dto.CurrentPeriodEnd,
            TrialEndsAt = dto.TrialEndsAt,
            CreatedAt = now
        };

        AddHistory(subscription, SubscriptionAction.Created, null, dto.PlanId, "Assinatura criada.");

        await _subscriptionRepository.AddAsync(subscription);
        await _subscriptionRepository.SaveChangesAsync();

        subscription.Plan = plan;
        return _mapper.Map<OwnerSubscriptionResponseDTO>(subscription);
    }

    public async Task<OwnerSubscriptionResponseDTO> GetByIdAsync(Guid subscriptionId)
    {
        return _mapper.Map<OwnerSubscriptionResponseDTO>(await GetOwnedSubscriptionOrThrow(subscriptionId));
    }

    public async Task<OwnerSubscriptionResponseDTO> GetActiveByOwnerAsync()
    {
        var subscription = await _subscriptionRepository.GetActiveByOwnerIdAsync(_userContext.OwnerId);
        if (subscription is null)
            throw new NotFoundException("Assinatura ativa não encontrada.");

        return _mapper.Map<OwnerSubscriptionResponseDTO>(subscription);
    }

    public async Task<PagedResult<OwnerSubscriptionResponseDTO>> GetPagedByOwnerAsync(PaginationParams paginationParams)
    {
        var subscriptions = await _subscriptionRepository.GetPagedByOwnerIdAsync(_userContext.OwnerId, paginationParams);
        var data = _mapper.Map<IEnumerable<OwnerSubscriptionResponseDTO>>(subscriptions.Data);
        return new PagedResult<OwnerSubscriptionResponseDTO>(
            data,
            subscriptions.TotalCount,
            subscriptions.PageNumber,
            subscriptions.PageSize);
    }

    public async Task<OwnerSubscriptionResponseDTO> ChangePlanAsync(Guid subscriptionId, ChangePlanDTO dto)
    {
        EnsureAdmin();

        var subscription = await GetOwnedSubscriptionOrThrow(subscriptionId);
        var newPlan = await _planRepository.GetByIdAsync(dto.PlanId);

        if (newPlan is null)
            throw new NotFoundException("Plano não encontrado.");

        if (!newPlan.IsActive)
            throw new BadRequestException("Não é possível mudar para um plano inativo.");

        var oldPlanId = subscription.PlanId;
        subscription.PlanId = dto.PlanId;
        subscription.Plan = newPlan;
        subscription.UpdatedAt = DateTime.UtcNow;
        AddHistory(subscription, SubscriptionAction.PlanChanged, oldPlanId, dto.PlanId, dto.Reason ?? "Plano alterado.");

        _subscriptionRepository.Update(subscription);
        await _subscriptionRepository.SaveChangesAsync();

        return _mapper.Map<OwnerSubscriptionResponseDTO>(subscription);
    }

    public async Task<OwnerSubscriptionResponseDTO> CancelAsync(Guid subscriptionId, CancelSubscriptionDTO dto)
    {
        EnsureAdmin();

        var subscription = await GetOwnedSubscriptionOrThrow(subscriptionId);
        subscription.Status = SubscriptionStatus.Canceled;
        subscription.CanceledAt = DateTime.UtcNow;
        subscription.CancelReason = dto.CancelReason;
        subscription.UpdatedAt = DateTime.UtcNow;
        AddHistory(subscription, SubscriptionAction.Canceled, subscription.PlanId, subscription.PlanId, dto.CancelReason ?? "Assinatura cancelada.");

        _subscriptionRepository.Update(subscription);
        await _subscriptionRepository.SaveChangesAsync();

        return _mapper.Map<OwnerSubscriptionResponseDTO>(subscription);
    }

    public async Task<OwnerSubscriptionResponseDTO> SuspendAsync(Guid subscriptionId, OwnerSubscriptionUpdateStatusDTO dto)
    {
        EnsureAdmin();

        var subscription = await GetOwnedSubscriptionOrThrow(subscriptionId);
        subscription.Status = SubscriptionStatus.Suspended;
        subscription.SuspendedAt = DateTime.UtcNow;
        subscription.UpdatedAt = DateTime.UtcNow;
        AddHistory(subscription, SubscriptionAction.Suspended, subscription.PlanId, subscription.PlanId, dto.Reason ?? "Assinatura suspensa.");

        _subscriptionRepository.Update(subscription);
        await _subscriptionRepository.SaveChangesAsync();

        return _mapper.Map<OwnerSubscriptionResponseDTO>(subscription);
    }

    public async Task<OwnerSubscriptionResponseDTO> ReactivateAsync(Guid subscriptionId)
    {
        EnsureAdmin();

        var subscription = await GetOwnedSubscriptionOrThrow(subscriptionId);
        subscription.Status = SubscriptionStatus.Active;
        subscription.SuspendedAt = null;
        subscription.CanceledAt = null;
        subscription.CancelReason = null;
        subscription.UpdatedAt = DateTime.UtcNow;
        AddHistory(subscription, SubscriptionAction.Reactivated, subscription.PlanId, subscription.PlanId, "Assinatura reativada.");

        _subscriptionRepository.Update(subscription);
        await _subscriptionRepository.SaveChangesAsync();

        return _mapper.Map<OwnerSubscriptionResponseDTO>(subscription);
    }

    public async Task<OwnerSubscriptionResponseDTO> AdminAssignPlanAsync(ManualAssignPlanDTO dto)
    {
        EnsureAdmin();

        var plan = await GetActivePlanOrThrow(dto.PlanId);

        if (await _subscriptionRepository.HasActiveOrTrialByOwnerIdAsync(dto.OwnerId))
            throw new BadRequestException("Este owner já possui assinatura ativa ou trial. Use a troca de plano em vez de criar outra assinatura.");

        var now = DateTime.UtcNow;
        var periodStart = dto.CurrentPeriodStart ?? now;
        var subscription = new OwnerSubscription
        {
            OwnerSubscriptionId = Guid.NewGuid(),
            OwnerId = dto.OwnerId,
            PlanId = dto.PlanId,
            Plan = plan,
            Status = dto.TrialEndsAt.HasValue && dto.TrialEndsAt.Value > now
                ? SubscriptionStatus.Trial
                : SubscriptionStatus.Active,
            BillingCycle = dto.BillingCycle,
            StartedAt = now,
            CurrentPeriodStart = periodStart,
            CurrentPeriodEnd = dto.CurrentPeriodEnd ?? CalculatePeriodEnd(periodStart, dto.BillingCycle),
            TrialEndsAt = dto.TrialEndsAt,
            CreatedAt = now
        };

        AddHistory(
            subscription,
            SubscriptionAction.Created,
            null,
            dto.PlanId,
            string.IsNullOrWhiteSpace(dto.Notes)
                ? "Plano atribuído manualmente pelo administrador."
                : $"Plano atribuído manualmente pelo administrador. Observação: {dto.Notes}");

        await _subscriptionRepository.AddAsync(subscription);
        await _subscriptionRepository.SaveChangesAsync();

        return _mapper.Map<OwnerSubscriptionResponseDTO>(subscription);
    }

    public async Task<OwnerSubscriptionResponseDTO> AdminChangeOwnerPlanAsync(AdminChangeOwnerPlanDTO dto)
    {
        EnsureAdmin();

        var subscription = await _subscriptionRepository.GetActiveByOwnerIdAsync(dto.OwnerId);
        if (subscription is null)
            throw new NotFoundException("Assinatura ativa não encontrada para este owner.");

        var plan = await GetActivePlanOrThrow(dto.NewPlanId);
        var oldPlanId = subscription.PlanId;
        var now = DateTime.UtcNow;

        subscription.PlanId = dto.NewPlanId;
        subscription.Plan = plan;
        subscription.BillingCycle = dto.BillingCycle;
        subscription.CurrentPeriodStart = now;
        subscription.CurrentPeriodEnd = CalculatePeriodEnd(now, dto.BillingCycle);
        subscription.UpdatedAt = now;

        AddHistory(
            subscription,
            SubscriptionAction.PlanChanged,
            oldPlanId,
            dto.NewPlanId,
            dto.Reason ?? "Plano alterado pelo administrador.");

        _subscriptionRepository.Update(subscription);
        await _subscriptionRepository.SaveChangesAsync();

        return _mapper.Map<OwnerSubscriptionResponseDTO>(subscription);
    }

    public async Task<OwnerSubscriptionResponseDTO> AdminCancelOwnerSubscriptionAsync(AdminCancelSubscriptionDTO dto)
    {
        EnsureAdmin();

        var subscription = await GetActiveOrLatestByOwnerOrThrow(dto.OwnerId);
        subscription.Status = SubscriptionStatus.Canceled;
        subscription.CanceledAt = DateTime.UtcNow;
        subscription.CancelReason = dto.Reason;
        subscription.UpdatedAt = DateTime.UtcNow;

        AddHistory(subscription, SubscriptionAction.Canceled, subscription.PlanId, subscription.PlanId, dto.Reason ?? "Assinatura cancelada pelo administrador.");

        _subscriptionRepository.Update(subscription);
        await _subscriptionRepository.SaveChangesAsync();

        return _mapper.Map<OwnerSubscriptionResponseDTO>(subscription);
    }

    public async Task<OwnerSubscriptionResponseDTO> AdminSuspendOwnerSubscriptionAsync(AdminSuspendSubscriptionDTO dto)
    {
        EnsureAdmin();

        var subscription = await GetActiveOrLatestByOwnerOrThrow(dto.OwnerId);
        subscription.Status = SubscriptionStatus.Suspended;
        subscription.SuspendedAt = DateTime.UtcNow;
        subscription.UpdatedAt = DateTime.UtcNow;

        AddHistory(subscription, SubscriptionAction.Suspended, subscription.PlanId, subscription.PlanId, dto.Reason ?? "Assinatura suspensa pelo administrador.");

        _subscriptionRepository.Update(subscription);
        await _subscriptionRepository.SaveChangesAsync();

        return _mapper.Map<OwnerSubscriptionResponseDTO>(subscription);
    }

    public async Task<OwnerSubscriptionResponseDTO> AdminReactivateOwnerSubscriptionAsync(AdminReactivateSubscriptionDTO dto)
    {
        EnsureAdmin();

        var subscription = await GetActiveOrLatestByOwnerOrThrow(dto.OwnerId);
        var activeSubscription = await _subscriptionRepository.GetActiveByOwnerIdAsync(dto.OwnerId);
        if (activeSubscription is not null && activeSubscription.OwnerSubscriptionId != subscription.OwnerSubscriptionId)
            throw new BadRequestException("Este owner já possui outra assinatura ativa ou trial.");

        subscription.Status = SubscriptionStatus.Active;
        subscription.SuspendedAt = null;
        subscription.CanceledAt = null;
        subscription.CancelReason = null;
        subscription.UpdatedAt = DateTime.UtcNow;

        AddHistory(subscription, SubscriptionAction.Reactivated, subscription.PlanId, subscription.PlanId, dto.Reason ?? "Assinatura reativada pelo administrador.");

        _subscriptionRepository.Update(subscription);
        await _subscriptionRepository.SaveChangesAsync();

        return _mapper.Map<OwnerSubscriptionResponseDTO>(subscription);
    }

    public async Task<PagedResult<OwnerSubscriptionResponseDTO>> AdminGetPagedAsync(PaginationParams paginationParams)
    {
        EnsureAdmin();

        var subscriptions = await _subscriptionRepository.GetPagedAsync(paginationParams);
        var data = _mapper.Map<IEnumerable<OwnerSubscriptionResponseDTO>>(subscriptions.Data);
        return new PagedResult<OwnerSubscriptionResponseDTO>(data, subscriptions.TotalCount, subscriptions.PageNumber, subscriptions.PageSize);
    }

    public async Task<PagedResult<OwnerSubscriptionResponseDTO>> AdminGetByOwnerAsync(Guid ownerId, PaginationParams paginationParams)
    {
        EnsureAdmin();

        var subscriptions = await _subscriptionRepository.GetPagedByOwnerIdAsync(ownerId, paginationParams);
        var data = _mapper.Map<IEnumerable<OwnerSubscriptionResponseDTO>>(subscriptions.Data);
        return new PagedResult<OwnerSubscriptionResponseDTO>(data, subscriptions.TotalCount, subscriptions.PageNumber, subscriptions.PageSize);
    }

    private async Task<OwnerSubscription> GetOwnedSubscriptionOrThrow(Guid subscriptionId)
    {
        var subscription = await _subscriptionRepository.GetByIdAsync(subscriptionId);
        if (subscription is null)
            throw new NotFoundException("Assinatura não encontrada.");

        if (subscription.OwnerId != _userContext.OwnerId)
            throw new UnauthorizedException();

        return subscription;
    }

    private async Task<OwnerSubscription> GetActiveOrLatestByOwnerOrThrow(Guid ownerId)
    {
        var subscription = await _subscriptionRepository.GetActiveByOwnerIdAsync(ownerId)
                           ?? await _subscriptionRepository.GetLatestByOwnerIdAsync(ownerId);

        if (subscription is null)
            throw new NotFoundException("Assinatura não encontrada para este owner.");

        return subscription;
    }

    private async Task<Plan> GetActivePlanOrThrow(Guid planId)
    {
        var plan = await _planRepository.GetByIdAsync(planId);
        if (plan is null)
            throw new NotFoundException("Plano não encontrado.");

        if (!plan.IsActive)
            throw new BadRequestException("Não é possível usar um plano inativo.");

        return plan;
    }

    private void EnsureAdmin()
    {
        if (!_userContext.IsAdmin && !_userContext.Roles.Contains("Admin"))
            throw new UnauthorizedException("Apenas administradores podem executar esta operação.");
    }

    private static DateTime CalculatePeriodEnd(DateTime periodStart, BillingCycle billingCycle)
    {
        return billingCycle == BillingCycle.Yearly
            ? periodStart.AddYears(1)
            : periodStart.AddMonths(1);
    }

    private void AddHistory(
        OwnerSubscription subscription,
        SubscriptionAction action,
        Guid? oldPlanId,
        Guid? newPlanId,
        string? description)
    {
        subscription.History.Add(new SubscriptionHistory
        {
            SubscriptionHistoryId = Guid.NewGuid(),
            Action = action,
            OldPlanId = oldPlanId,
            NewPlanId = newPlanId,
            Description = description,
            ChangedByUserId = _userContext.UserId == Guid.Empty ? null : _userContext.UserId,
            CreatedAt = DateTime.UtcNow
        });
    }
}

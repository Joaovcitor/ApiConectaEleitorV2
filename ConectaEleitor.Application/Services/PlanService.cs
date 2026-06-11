using AutoMapper;
using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.DTOs.Plans;
using ConectaEleitor.Application.Interfaces;
using ConectaEleitor.Domain.Entities;
using ConectaEleitor.Domain.Exceptions;

namespace ConectaEleitor.Application.Services;

public class PlanService : IPlanService
{
    private readonly IPlanRepository _planRepository;
    private readonly IMapper _mapper;

    public PlanService(IPlanRepository planRepository, IMapper mapper)
    {
        _planRepository = planRepository;
        _mapper = mapper;
    }

    public async Task<PlanResponseDTO> CreateAsync(PlanCreateDTO dto)
    {
        dto.Slug = NormalizeSlug(dto.Slug);

        if (await _planRepository.ExistsBySlugAsync(dto.Slug))
            throw new BadRequestException("Já existe um plano com este slug.");

        var plan = _mapper.Map<Plan>(dto);
        plan.PlanId = Guid.NewGuid();
        plan.CreatedAt = DateTime.UtcNow;

        foreach (var feature in plan.Features)
            feature.PlanFeatureId = Guid.NewGuid();

        foreach (var limit in plan.Limits)
            limit.PlanLimitId = Guid.NewGuid();

        await _planRepository.AddAsync(plan);
        await _planRepository.SaveChangesAsync();

        return _mapper.Map<PlanResponseDTO>(plan);
    }

    public async Task<PlanResponseDTO> UpdateAsync(Guid planId, PlanUpdateDTO dto)
    {
        var plan = await GetPlanOrThrow(planId);
        dto.Slug = NormalizeSlug(dto.Slug);

        if (await _planRepository.ExistsBySlugAsync(dto.Slug, planId))
            throw new BadRequestException("Já existe um plano com este slug.");

        _mapper.Map(dto, plan);
        plan.UpdatedAt = DateTime.UtcNow;

        _planRepository.Update(plan);
        await _planRepository.SaveChangesAsync();

        return _mapper.Map<PlanResponseDTO>(plan);
    }

    public async Task DeleteAsync(Guid planId)
    {
        var plan = await GetPlanOrThrow(planId);

        if (await _planRepository.HasSubscriptionsAsync(planId))
            throw new BadRequestException("Não é possível excluir um plano com assinaturas vinculadas.");

        _planRepository.Delete(plan);
        await _planRepository.SaveChangesAsync();
    }

    public async Task<PlanResponseDTO> GetByIdAsync(Guid planId)
    {
        return _mapper.Map<PlanResponseDTO>(await GetPlanOrThrow(planId));
    }

    public async Task<PlanResponseDTO> GetBySlugAsync(string slug)
    {
        var plan = await _planRepository.GetBySlugAsync(NormalizeSlug(slug));
        if (plan is null)
            throw new NotFoundException("Plano não encontrado.");

        return _mapper.Map<PlanResponseDTO>(plan);
    }

    public async Task<PagedResult<PlanResponseDTO>> GetPagedAsync(PaginationParams paginationParams, bool onlyActive = true)
    {
        var plans = await _planRepository.GetPagedAsync(paginationParams, onlyActive);
        var data = _mapper.Map<IEnumerable<PlanResponseDTO>>(plans.Data);
        return new PagedResult<PlanResponseDTO>(data, plans.TotalCount, plans.PageNumber, plans.PageSize);
    }

    public async Task<PagedResult<PlanResponseDTO>> GetPublicPagedAsync(PaginationParams paginationParams)
    {
        var plans = await _planRepository.GetPublicPagedAsync(paginationParams);
        var data = _mapper.Map<IEnumerable<PlanResponseDTO>>(plans.Data);
        return new PagedResult<PlanResponseDTO>(data, plans.TotalCount, plans.PageNumber, plans.PageSize);
    }

    public async Task<PlanResponseDTO> ActivateAsync(Guid planId)
    {
        var plan = await GetPlanOrThrow(planId);
        plan.IsActive = true;
        plan.UpdatedAt = DateTime.UtcNow;
        _planRepository.Update(plan);
        await _planRepository.SaveChangesAsync();
        return _mapper.Map<PlanResponseDTO>(plan);
    }

    public async Task<PlanResponseDTO> DeactivateAsync(Guid planId)
    {
        var plan = await GetPlanOrThrow(planId);
        plan.IsActive = false;
        plan.UpdatedAt = DateTime.UtcNow;
        _planRepository.Update(plan);
        await _planRepository.SaveChangesAsync();
        return _mapper.Map<PlanResponseDTO>(plan);
    }

    private async Task<Plan> GetPlanOrThrow(Guid planId)
    {
        var plan = await _planRepository.GetByIdAsync(planId);
        if (plan is null)
            throw new NotFoundException("Plano não encontrado.");

        return plan;
    }

    private static string NormalizeSlug(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
            throw new BadRequestException("O slug do plano é obrigatório.");

        return slug.Trim().ToLowerInvariant();
    }
}

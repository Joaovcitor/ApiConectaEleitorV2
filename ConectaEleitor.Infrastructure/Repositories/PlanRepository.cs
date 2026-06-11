using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.Interfaces;
using ConectaEleitor.Domain.Entities;
using ConectaEleitor.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ConectaEleitor.Infrastructure.Repositories;

public class PlanRepository : IPlanRepository
{
    private readonly AppDbContext _context;

    public PlanRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Plan?> GetByIdAsync(Guid planId)
    {
        return await _context.Plans
            .Include(x => x.Features)
            .Include(x => x.Limits)
            .FirstOrDefaultAsync(x => x.PlanId == planId);
    }

    public async Task<Plan?> GetBySlugAsync(string slug)
    {
        return await _context.Plans
            .Include(x => x.Features)
            .Include(x => x.Limits)
            .FirstOrDefaultAsync(x => x.Slug == slug);
    }

    public async Task<bool> ExistsBySlugAsync(string slug, Guid? ignorePlanId = null)
    {
        return await _context.Plans
            .AsNoTracking()
            .AnyAsync(x => x.Slug == slug && (!ignorePlanId.HasValue || x.PlanId != ignorePlanId.Value));
    }

    public async Task<bool> HasSubscriptionsAsync(Guid planId)
    {
        return await _context.OwnerSubscriptions
            .AsNoTracking()
            .AnyAsync(x => x.PlanId == planId);
    }

    public async Task<PagedResult<Plan>> GetPagedAsync(
        PaginationParams paginationParams,
        bool onlyActive = true)
    {
        var query = _context.Plans
            .Include(x => x.Features)
            .Include(x => x.Limits)
            .AsNoTracking()
            .AsQueryable();

        if (onlyActive)
            query = query.Where(x => x.IsActive);

        var totalCount = await query.CountAsync();

        var data = await query
            .OrderBy(x => x.DisplayOrder)
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

        return new PagedResult<Plan>(
            data,
            totalCount,
            paginationParams.PageNumber,
            paginationParams.PageSize
        );
    }

    public async Task<PagedResult<Plan>> GetPublicPagedAsync(PaginationParams paginationParams)
    {
        var query = _context.Plans
            .Include(x => x.Features)
            .Include(x => x.Limits)
            .AsNoTracking()
            .Where(x => x.IsActive && x.IsPublic);

        var totalCount = await query.CountAsync();

        var data = await query
            .OrderBy(x => x.DisplayOrder)
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

        return new PagedResult<Plan>(
            data,
            totalCount,
            paginationParams.PageNumber,
            paginationParams.PageSize);
    }

    public async Task AddAsync(Plan plan)
    {
        await _context.Plans.AddAsync(plan);
    }

    public void Update(Plan plan)
    {
        _context.Plans.Update(plan);
    }

    public void Delete(Plan plan)
    {
        _context.Plans.Remove(plan);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}

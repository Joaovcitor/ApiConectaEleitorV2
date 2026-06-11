using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.Interfaces;
using ConectaEleitor.Domain.Entities;
using ConectaEleitor.Domain.Enums;
using ConectaEleitor.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ConectaEleitor.Infrastructure.Repositories;

public class OwnerSubscriptionRepository : IOwnerSubscriptionRepository
{
    private readonly AppDbContext _context;

    public OwnerSubscriptionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<OwnerSubscription?> GetByIdAsync(Guid subscriptionId)
    {
        return await _context.OwnerSubscriptions
            .Include(x => x.Plan)
                .ThenInclude(x => x.Features)
            .Include(x => x.Plan)
                .ThenInclude(x => x.Limits)
            .Include(x => x.History)
            .Include(x => x.Payments)
            .FirstOrDefaultAsync(x => x.OwnerSubscriptionId == subscriptionId);
    }

    public async Task<OwnerSubscription?> GetActiveByOwnerIdAsync(Guid ownerId)
    {
        return await _context.OwnerSubscriptions
            .Include(x => x.Plan)
                .ThenInclude(x => x.Features)
            .Include(x => x.Plan)
                .ThenInclude(x => x.Limits)
            .Include(x => x.History)
            .FirstOrDefaultAsync(x =>
                x.OwnerId == ownerId &&
                (x.Status == SubscriptionStatus.Active ||
                 x.Status == SubscriptionStatus.Trial));
    }

    public async Task<OwnerSubscription?> GetLatestByOwnerIdAsync(Guid ownerId)
    {
        return await _context.OwnerSubscriptions
            .Include(x => x.Plan)
                .ThenInclude(x => x.Features)
            .Include(x => x.Plan)
                .ThenInclude(x => x.Limits)
            .Include(x => x.History)
            .Where(x => x.OwnerId == ownerId)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<OwnerSubscription?> GetByOwnerIdAndIdAsync(Guid ownerId, Guid subscriptionId)
    {
        return await _context.OwnerSubscriptions
            .Include(x => x.Plan)
                .ThenInclude(x => x.Features)
            .Include(x => x.Plan)
                .ThenInclude(x => x.Limits)
            .Include(x => x.History)
            .Include(x => x.Payments)
            .FirstOrDefaultAsync(x => x.OwnerId == ownerId && x.OwnerSubscriptionId == subscriptionId);
    }

    public async Task<bool> HasActiveOrTrialByOwnerIdAsync(Guid ownerId)
    {
        return await _context.OwnerSubscriptions
            .AsNoTracking()
            .AnyAsync(x =>
                x.OwnerId == ownerId &&
                (x.Status == SubscriptionStatus.Active ||
                 x.Status == SubscriptionStatus.Trial));
    }

    public async Task<PagedResult<OwnerSubscription>> GetPagedByOwnerIdAsync(
        Guid ownerId,
        PaginationParams paginationParams)
    {
        var query = _context.OwnerSubscriptions
            .Include(x => x.Plan)
            .AsNoTracking()
            .Where(x => x.OwnerId == ownerId)
            .OrderByDescending(x => x.CreatedAt);

        var totalCount = await query.CountAsync();

        var data = await query
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

        return new PagedResult<OwnerSubscription>(
            data,
            totalCount,
            paginationParams.PageNumber,
            paginationParams.PageSize);
    }

    public async Task<PagedResult<OwnerSubscription>> GetPagedAsync(PaginationParams paginationParams)
    {
        var query = _context.OwnerSubscriptions
            .Include(x => x.Plan)
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt);

        var totalCount = await query.CountAsync();

        var data = await query
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

        return new PagedResult<OwnerSubscription>(
            data,
            totalCount,
            paginationParams.PageNumber,
            paginationParams.PageSize);
    }

    public async Task AddAsync(OwnerSubscription subscription)
    {
        await _context.OwnerSubscriptions.AddAsync(subscription);
    }

    public void Update(OwnerSubscription subscription)
    {
        _context.OwnerSubscriptions.Update(subscription);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}

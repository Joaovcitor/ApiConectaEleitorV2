using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.DTOs.Payments;
using ConectaEleitor.Application.Interfaces;
using ConectaEleitor.Domain.Entities;
using ConectaEleitor.Domain.Enums;
using ConectaEleitor.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ConectaEleitor.Infrastructure.Repositories;

public class PaymentTransactionRepository : IPaymentTransactionRepository
{
    private readonly AppDbContext _context;

    public PaymentTransactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PaymentTransaction?> GetByIdAsync(Guid paymentId)
    {
        return await _context.PaymentTransactions
            .Include(x => x.Plan)
            .Include(x => x.OwnerSubscription)
            .FirstOrDefaultAsync(x => x.PaymentTransactionId == paymentId);
    }

    public async Task<PaymentTransaction?> GetByExternalPaymentIdAsync(string externalPaymentId)
    {
        return await _context.PaymentTransactions
            .Include(x => x.Plan)
            .Include(x => x.OwnerSubscription)
            .FirstOrDefaultAsync(x => x.ExternalPaymentId == externalPaymentId);
    }

    public async Task<PagedResult<PaymentTransaction>> GetPagedByOwnerIdAsync(
        Guid ownerId,
        PaymentTransactionFilterDTO filter,
        PaginationParams paginationParams)
    {
        var query = _context.PaymentTransactions
            .Include(x => x.Plan)
            .AsNoTracking()
            .Where(x => x.OwnerId == ownerId);

        if (filter.Status.HasValue)
            query = query.Where(x => x.Status == filter.Status.Value);

        if (filter.Provider.HasValue)
            query = query.Where(x => x.Provider == filter.Provider.Value);

        if (filter.PaymentMethod.HasValue)
            query = query.Where(x => x.PaymentMethod == filter.PaymentMethod.Value);

        if (filter.StartDate.HasValue)
            query = query.Where(x => x.CreatedAt >= filter.StartDate.Value);

        if (filter.EndDate.HasValue)
            query = query.Where(x => x.CreatedAt <= filter.EndDate.Value);

        if (filter.SubscriptionId.HasValue)
            query = query.Where(x => x.OwnerSubscriptionId == filter.SubscriptionId.Value);

        var totalCount = await query.CountAsync();

        var data = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

        return new PagedResult<PaymentTransaction>(
            data,
            totalCount,
            paginationParams.PageNumber,
            paginationParams.PageSize
        );
    }

    public async Task<PagedResult<PaymentTransaction>> GetPagedBySubscriptionIdAsync(
        Guid subscriptionId,
        PaginationParams paginationParams)
    {
        var query = _context.PaymentTransactions
            .Include(x => x.Plan)
            .AsNoTracking()
            .Where(x => x.OwnerSubscriptionId == subscriptionId)
            .OrderByDescending(x => x.CreatedAt);
        var totalCount = await query.CountAsync();
        var data = await query
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();
        return new PagedResult<PaymentTransaction>(
            data,
            totalCount,
            paginationParams.PageNumber,
            paginationParams.PageSize);
    }

    public async Task<List<PaymentTransaction>> GetPendingByOwnerIdAsync(Guid ownerId)
    {
        return await _context.PaymentTransactions
            .Where(x =>
                x.OwnerId == ownerId &&
                (x.Status == PaymentStatus.Pending ||
                 x.Status == PaymentStatus.WaitingPayment))
            .ToListAsync();
    }

    public async Task AddAsync(PaymentTransaction payment)
    {
        await _context.PaymentTransactions.AddAsync(payment);
    }

    public void Update(PaymentTransaction payment)
    {
        _context.PaymentTransactions.Update(payment);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}

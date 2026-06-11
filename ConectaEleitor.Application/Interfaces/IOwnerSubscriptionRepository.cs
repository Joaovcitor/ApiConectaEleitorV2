using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Domain.Entities;

namespace ConectaEleitor.Application.Interfaces;

public interface IOwnerSubscriptionRepository
{
    Task<OwnerSubscription?> GetByIdAsync(Guid subscriptionId);

    Task<OwnerSubscription?> GetActiveByOwnerIdAsync(Guid ownerId);

    Task<OwnerSubscription?> GetLatestByOwnerIdAsync(Guid ownerId);

    Task<OwnerSubscription?> GetByOwnerIdAndIdAsync(Guid ownerId, Guid subscriptionId);

    Task<bool> HasActiveOrTrialByOwnerIdAsync(Guid ownerId);

    Task<PagedResult<OwnerSubscription>> GetPagedAsync(PaginationParams paginationParams);

    Task<PagedResult<OwnerSubscription>> GetPagedByOwnerIdAsync(
        Guid ownerId,
        PaginationParams paginationParams);

    Task AddAsync(OwnerSubscription subscription);

    void Update(OwnerSubscription subscription);

    Task SaveChangesAsync();
}

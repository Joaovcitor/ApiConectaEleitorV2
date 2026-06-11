using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.DTOs.Payments;
using ConectaEleitor.Domain.Entities;

namespace ConectaEleitor.Application.Interfaces;

public interface IPaymentTransactionRepository
{
    Task<PaymentTransaction?> GetByIdAsync(Guid paymentId);

    Task<PaymentTransaction?> GetByExternalPaymentIdAsync(string externalPaymentId);

    Task<PagedResult<PaymentTransaction>> GetPagedByOwnerIdAsync(
        Guid ownerId,
        PaymentTransactionFilterDTO filter,
        PaginationParams paginationParams);

    Task<PagedResult<PaymentTransaction>> GetPagedBySubscriptionIdAsync(
        Guid subscriptionId,
        PaginationParams paginationParams);

    Task<List<PaymentTransaction>> GetPendingByOwnerIdAsync(Guid ownerId);

    Task AddAsync(PaymentTransaction payment);

    void Update(PaymentTransaction payment);

    Task SaveChangesAsync();
}

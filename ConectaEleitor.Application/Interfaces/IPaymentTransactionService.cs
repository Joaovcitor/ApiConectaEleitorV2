using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.DTOs.Payments;

namespace ConectaEleitor.Application.Interfaces;

public interface IPaymentTransactionService
{
    Task<PaymentTransactionResponseDTO> CreateAsync(PaymentTransactionCreateDTO dto);
    Task<PaymentTransactionResponseDTO> GetByIdAsync(Guid paymentId);
    Task<PagedResult<PaymentTransactionResponseDTO>> GetPagedByOwnerAsync(
        PaymentTransactionFilterDTO filter,
        PaginationParams paginationParams);
    Task<PagedResult<PaymentTransactionResponseDTO>> AdminGetPagedByOwnerAsync(
        Guid ownerId,
        PaymentTransactionFilterDTO filter,
        PaginationParams paginationParams);
    Task<PagedResult<PaymentTransactionResponseDTO>> GetPagedBySubscriptionAsync(
        Guid subscriptionId,
        PaginationParams paginationParams);
    Task<IEnumerable<PaymentTransactionResponseDTO>> GetPendingByOwnerAsync();
    Task<PaymentTransactionResponseDTO> MarkAsPaidAsync(Guid paymentId);
    Task<PaymentTransactionResponseDTO> MarkAsFailedAsync(Guid paymentId, PaymentTransactionUpdateStatusDTO dto);
    Task<PaymentTransactionResponseDTO> CancelAsync(Guid paymentId);
    Task<PaymentTransactionResponseDTO> RefundAsync(Guid paymentId);
}

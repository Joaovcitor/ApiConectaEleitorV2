using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.DTOs.Subscriptions;

namespace ConectaEleitor.Application.Interfaces;

public interface IOwnerSubscriptionService
{
    Task<OwnerSubscriptionResponseDTO> CreateAsync(OwnerSubscriptionCreateDTO dto);
    Task<OwnerSubscriptionResponseDTO> GetByIdAsync(Guid subscriptionId);
    Task<OwnerSubscriptionResponseDTO> GetActiveByOwnerAsync();
    Task<PagedResult<OwnerSubscriptionResponseDTO>> GetPagedByOwnerAsync(PaginationParams paginationParams);
    Task<OwnerSubscriptionResponseDTO> ChangePlanAsync(Guid subscriptionId, ChangePlanDTO dto);
    Task<OwnerSubscriptionResponseDTO> CancelAsync(Guid subscriptionId, CancelSubscriptionDTO dto);
    Task<OwnerSubscriptionResponseDTO> SuspendAsync(Guid subscriptionId, OwnerSubscriptionUpdateStatusDTO dto);
    Task<OwnerSubscriptionResponseDTO> ReactivateAsync(Guid subscriptionId);
    Task<OwnerSubscriptionResponseDTO> AdminAssignPlanAsync(ManualAssignPlanDTO dto);
    Task<OwnerSubscriptionResponseDTO> AdminChangeOwnerPlanAsync(AdminChangeOwnerPlanDTO dto);
    Task<OwnerSubscriptionResponseDTO> AdminCancelOwnerSubscriptionAsync(AdminCancelSubscriptionDTO dto);
    Task<OwnerSubscriptionResponseDTO> AdminSuspendOwnerSubscriptionAsync(AdminSuspendSubscriptionDTO dto);
    Task<OwnerSubscriptionResponseDTO> AdminReactivateOwnerSubscriptionAsync(AdminReactivateSubscriptionDTO dto);
    Task<PagedResult<OwnerSubscriptionResponseDTO>> AdminGetPagedAsync(PaginationParams paginationParams);
    Task<PagedResult<OwnerSubscriptionResponseDTO>> AdminGetByOwnerAsync(Guid ownerId, PaginationParams paginationParams);
}

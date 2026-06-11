using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.DTOs.Plans;

namespace ConectaEleitor.Application.Interfaces;

public interface IPlanService
{
    Task<PlanResponseDTO> CreateAsync(PlanCreateDTO dto);
    Task<PlanResponseDTO> UpdateAsync(Guid planId, PlanUpdateDTO dto);
    Task DeleteAsync(Guid planId);
    Task<PlanResponseDTO> GetByIdAsync(Guid planId);
    Task<PlanResponseDTO> GetBySlugAsync(string slug);
    Task<PagedResult<PlanResponseDTO>> GetPagedAsync(PaginationParams paginationParams, bool onlyActive = true);
    Task<PagedResult<PlanResponseDTO>> GetPublicPagedAsync(PaginationParams paginationParams);
    Task<PlanResponseDTO> ActivateAsync(Guid planId);
    Task<PlanResponseDTO> DeactivateAsync(Guid planId);
}

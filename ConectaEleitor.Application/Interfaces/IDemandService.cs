using ConectaEleitor.Application.DTOs.DemandsDTOs;
using ConectaEleitor.Application.DTOs.Pagination;

namespace ConectaEleitor.Application.Interfaces;

public interface IDemandService
{
    Task<DemandResponseDTO> Create(DemandCreateDTO demandCreateDTO);
    Task<DemandResponseByIdDTO> GetById(Guid demandId);
    Task<PagedResult<DemandResponseDTO>> GetAll(PaginationParams paginationParams);
    Task<PagedResult<DemandResponseDTO>> GetAllDemandsByCitizenId(PaginationParams paginationParams, Guid citizenId);
    Task<DemandResponseByIdDTO> Update(DemandUpdateDTO demandUpdateDTO, Guid demandId);
    Task Delete(Guid demandId);
}
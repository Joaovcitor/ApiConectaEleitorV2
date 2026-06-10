using ConectaEleitor.Application.DTOs.CitizenDTOs;
using ConectaEleitor.Application.DTOs.Pagination;

namespace ConectaEleitor.Application.Interfaces;

public interface ICitizenService
{
    Task<CitizenResponseDTO> Create(CitizenCreateDTO citizen);
    Task<CitizenResponseByIdDTO> GetById(Guid citizenId);
    Task<PagedResult<CitizenResponseDTO>> GetAll(PaginationParams paginationParams);
    Task<PagedResult<CitizenResponseDTO>> GetPagedCitizensLeaderAsync(PaginationParams paginationParams);
    Task<CitizenResponseByIdDTO> Update(CitizenUpdateDTO citizenUpdateDto, Guid citizenId);
    Task<string> DeleteById(Guid id);
}
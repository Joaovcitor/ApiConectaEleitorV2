using ConectaEleitor.Application.DTOs.AgendasDTOs;
using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Domain.Entities;

namespace ConectaEleitor.Application.Interfaces;

public interface IAgendaService
{
    Task<AgendaResponseDTO> CreateAsync(AgendaCreateDTO agendaCreateDTO);
    Task<AgendaResponseByIdDTO> GetByIdAsync(Guid agendaId);
    Task<PagedResult<AgendaResponseDTO>> GetAll(PaginationParams paginationParams);
    Task<AgendaResponseDTO> UpdateAsync(Guid agendaId, AgendaUpdateDTO agendaUpdateDTO);
    Task DeleteAsync(Guid agendaId);
}
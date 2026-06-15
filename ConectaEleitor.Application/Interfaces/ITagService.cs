using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.DTOs.TagsDTOs;

namespace ConectaEleitor.Application.Interfaces;

public interface ITagService
{
    Task<TagResponseDTO> Create(TagCreateDTO tag);
    Task<TagResponseDTO> GetById(Guid tagId);
    Task<PagedResult<TagResponseDTO>> GetAll(PaginationParams paginationParams);
    Task<TagResponseDTO> Update(TagUpdateDTO tag, Guid tagId);
    Task Delete(Guid tagId);
}
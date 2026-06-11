using ConectaEleitor.Application.DTOs.AssemblymanPostsDTOs;
using ConectaEleitor.Application.DTOs.Pagination;

namespace ConectaEleitor.Application.Interfaces;

public interface IPostService
{
    Task<AssemblymanPostResponseDTO> Create(AssemblymanPostCreateDTO post);
    Task<AssemblymanPostResponseDTO> GetById(Guid assemblymanPostId);
    Task<PagedResult<AssemblymanPostResponseDTO>> GetAllPosts(PaginationParams paginationParams);
    Task<PagedResult<AssemblymanPostResponseDTO>> GetMyPosts(PaginationParams paginationParams);
    Task<AssemblymanPostResponseDTO> Update(AssemblymanPostUpdateDTO post, Guid assemblymanPostId);
    Task<string> Delete(Guid assemblymanPostId);
}
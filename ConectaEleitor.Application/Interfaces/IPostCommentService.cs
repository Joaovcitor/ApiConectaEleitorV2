using ConectaEleitor.Application.DTOs.AssemblymanPostsDTOs;
using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Domain.Entities;

namespace ConectaEleitor.Application.Interfaces;

public interface IPostCommentService
{
    Task<AssemblymanPostCommentResponseDTO> CreatePostComment(AssemblymanPostCommentCreateDTO comment);
    Task<AssemblymanPostCommentResponseDTO> GetById(Guid commentId);
    Task<PagedResult<AssemblymanPostCommentResponseDTO>> GetAllCommentsByPostId(PaginationParams paginationParams, Guid postId);
    Task<string> Delete(Guid commentId);
}
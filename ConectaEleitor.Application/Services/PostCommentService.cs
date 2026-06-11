using AutoMapper;
using ConectaEleitor.Application.DTOs.AssemblymanPostsDTOs;
using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.Interfaces;
using ConectaEleitor.Domain.Entities;
using ConectaEleitor.Domain.Exceptions;

namespace ConectaEleitor.Application.Services;

public class PostCommentService : IPostCommentService
{
    private readonly IUserContext _userContext;
    private readonly IPostCommentRepository _postCommentRepository;
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;

    public PostCommentService(IUserContext userContext, IPostCommentRepository postCommentRepository, IPostRepository postRepository, IMapper mapper)
    {
        _userContext = userContext;
        _postCommentRepository = postCommentRepository;
        _postRepository = postRepository;
        _mapper = mapper;
    }
    
    public async Task<AssemblymanPostCommentResponseDTO> CreatePostComment(AssemblymanPostCommentCreateDTO comment)
    {
        var authorId = _userContext.UserId;
        var postExist = await _postRepository.FindByIdAsync(comment.PostId);
        if (postExist is null)
        {
            throw new NotFoundException("Post não encontrado!");
        }
        var commentDto = _mapper.Map<AssemblymanPostComment>(comment);
        commentDto.UserId = authorId;
        commentDto.PostId = postExist.AssemblymanPostId;
        await _postCommentRepository.AddAsync(commentDto);
        await _postCommentRepository.SaveChanges();
        return _mapper.Map<AssemblymanPostCommentResponseDTO>(commentDto);
    }

    public async Task<AssemblymanPostCommentResponseDTO> GetById(Guid commentId)
    {
        var comment = await _postCommentRepository.GetCommentById(commentId);
        if (comment is null)
        {
            throw new NotFoundException("Comentário não encontrado!");
        }
        return _mapper.Map<AssemblymanPostCommentResponseDTO>(comment);
    }

    public async Task<PagedResult<AssemblymanPostCommentResponseDTO>> GetAllCommentsByPostId(PaginationParams paginationParams, Guid postId)
    {
        var comments = await _postCommentRepository.GetCommentsByPostId(postId, paginationParams);
        var data = _mapper.Map<IEnumerable<AssemblymanPostCommentResponseDTO>>(comments.Data);
        return new PagedResult<AssemblymanPostCommentResponseDTO>(data, comments.TotalCount, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public async Task<string> Delete(Guid commentId)
    {
        var userId = _userContext.UserId;
        var comment = await _postCommentRepository.GetCommentById(commentId);
        if (comment is null)
        {
            throw new NotFoundException("Comentário não econtrado");
        }

        if (comment.UserId != userId)
        {
            throw new UnauthorizedException();
        }
        _postCommentRepository.Delete(comment);
        await _postCommentRepository.SaveChanges();
        return "Comentário removido com sucesso!";
    }
}
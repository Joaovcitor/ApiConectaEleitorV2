using AutoMapper;
using ConectaEleitor.Application.DTOs.AssemblymanPostsDTOs;
using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.Interfaces;
using ConectaEleitor.Domain.Entities;
using ConectaEleitor.Domain.Exceptions;

namespace ConectaEleitor.Application.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;

    public PostService(IPostRepository postRepository, IUserContext userContext, IMapper mapper)
    {
        _postRepository = postRepository;
        _userContext = userContext;
        _mapper = mapper;
    }

    public async Task<AssemblymanPostResponseDTO> Create(AssemblymanPostCreateDTO post)
    {
        var ownerId =  _userContext.OwnerId;
        var userId = _userContext.UserId;
        var assemblymanPost = _mapper.Map<AssemblymanPost>(post);
        assemblymanPost.OwnerId = ownerId;
        assemblymanPost.UserId = userId;
        await _postRepository.AddAsync(assemblymanPost);
        await _postRepository.SaveChangesAsync();
        return _mapper.Map<AssemblymanPostResponseDTO>(assemblymanPost);
    }

    public async Task<AssemblymanPostResponseDTO> GetById(Guid assemblymanPostId)
    {
        var post = await _postRepository.FindByIdAsync(assemblymanPostId);
        if (post == null)
        {
            throw new NotFoundException("Post não encontrado");
        }
        return _mapper.Map<AssemblymanPostResponseDTO>(post);
    }

    public async Task<PagedResult<AssemblymanPostResponseDTO>> GetAllPosts(PaginationParams paginationParams)
    {
        var posts = await _postRepository.GetAllPagedAsync(paginationParams);
        var data = _mapper.Map<IEnumerable<AssemblymanPostResponseDTO>>(posts.Data);
        return new PagedResult<AssemblymanPostResponseDTO>(data, posts.TotalCount, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public async Task<PagedResult<AssemblymanPostResponseDTO>> GetMyPosts(PaginationParams paginationParams)
    {
        var ownerId = _userContext.OwnerId;
        var posts = await _postRepository.GetPostsUserLogged(paginationParams, ownerId);
        var data = _mapper.Map<IEnumerable<AssemblymanPostResponseDTO>>(posts.Data);
        return new PagedResult<AssemblymanPostResponseDTO>(data, posts.TotalCount, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public async Task<AssemblymanPostResponseDTO> Update(AssemblymanPostUpdateDTO post, Guid assemblymanPostId)
    {
        var ownerId = _userContext.OwnerId;
        var postExist = await _postRepository.FindByIdAsync(assemblymanPostId);
        if (postExist == null)
        {
            throw new NotFoundException("Post não encontrado");
        }

        if (postExist.OwnerId != ownerId)
        {
            throw new UnauthorizedException();
        }
        _mapper.Map(post, postExist);
        postExist.UpdatedAt = DateTime.UtcNow;
        _postRepository.Update(postExist);
        await _postRepository.SaveChangesAsync();
        return  _mapper.Map<AssemblymanPostResponseDTO>(postExist);
    }

    public async Task<string> Delete(Guid assemblymanPostId)
    {
        var ownerId = _userContext.OwnerId;
        var postExist = await _postRepository.FindByIdAsync(assemblymanPostId);
        if (postExist == null)
        {
            throw new NotFoundException("Post não encontrado");
        }

        if (postExist.OwnerId != ownerId)
        {
            throw new UnauthorizedException();
        }
        _postRepository.Delete(postExist);
        await _postRepository.SaveChangesAsync();
        return "Post deletado!";
    }
}
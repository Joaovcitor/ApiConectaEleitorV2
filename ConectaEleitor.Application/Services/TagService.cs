using AutoMapper;
using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.DTOs.TagsDTOs;
using ConectaEleitor.Application.Interfaces;
using ConectaEleitor.Domain.Entities;
using ConectaEleitor.Domain.Exceptions;

namespace ConectaEleitor.Application.Services;

public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;

    public TagService(ITagRepository tagRepository, IMapper mapper, IUserContext userContext)
    {
        _tagRepository = tagRepository;
        _mapper = mapper;
        _userContext = userContext;
    }

    public async Task<TagResponseDTO> Create(TagCreateDTO tag)
    {
        var ownerId = _userContext.OwnerId;
        var nameUpper = tag.Name.ToUpper();
        var nameExist = await _tagRepository.FindByNameAsync(nameUpper, ownerId);
        if (nameExist != null)
        {
            throw new BadRequestException("Você já possui uma TAG com esse nome, mantenha nomes únicos nas suas TAGs");
        }
        var tagDto = _mapper.Map<Tag>(tag);
        tagDto.OwnerId = ownerId;
        tagDto.Name = nameUpper;
        await _tagRepository.AddAsync(tagDto);
        await _tagRepository.SaveChangesAsync();
        return _mapper.Map<TagResponseDTO>(tagDto);
    }

    public async Task<TagResponseDTO> GetById(Guid tagId)
    {
        var ownerId = _userContext.OwnerId;
        var tag = await _tagRepository.GetByIdAsync(tagId, ownerId);
        if (tag == null)
        {
            throw new NotFoundException("TAG não encontrada!");
        }
        return _mapper.Map<TagResponseDTO>(tag);
    }

    public async Task<PagedResult<TagResponseDTO>> GetAll(PaginationParams paginationParams)
    {
        var ownerId = _userContext.OwnerId;
        var tags = await _tagRepository.GetAllAsync(paginationParams, ownerId);
        var data = _mapper.Map<IEnumerable<TagResponseDTO>>(tags.Data);
        return new PagedResult<TagResponseDTO>(data, tags.TotalCount, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public async Task<TagResponseDTO> Update(TagUpdateDTO tag, Guid tagId)
    {
        var ownerId = _userContext.OwnerId;
        var tagExist = await _tagRepository.GetByIdAsync(tagId, ownerId);
        if (tagExist == null)
        {
            throw new NotFoundException("TAG não encontrada");
        }
        var nameUpper = tag.Name.ToUpper();
        _mapper.Map(tag, tagExist);
        tagExist.UpdatedAt = DateTime.UtcNow;
        tagExist.Name = nameUpper;
        _tagRepository.Update(tagExist);
        await _tagRepository.SaveChangesAsync();
        return _mapper.Map<TagResponseDTO>(tagExist);
    }

    public async Task Delete(Guid tagId)
    {
        var ownerId = _userContext.OwnerId;
        var tagExist = await _tagRepository.GetByIdAsync(tagId, ownerId);
        if (tagExist == null)
        {
            throw new NotFoundException("TAG não encontrada");
        }
        _tagRepository.Remove(tagExist);
        await _tagRepository.SaveChangesAsync();
    }
}
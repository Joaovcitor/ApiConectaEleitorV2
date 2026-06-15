using AutoMapper;
using ConectaEleitor.Application.DTOs.TagCitizensDTOs;
using ConectaEleitor.Application.Interfaces;
using ConectaEleitor.Domain.Entities;
using ConectaEleitor.Domain.Exceptions;

namespace ConectaEleitor.Application.Services;

public class TagCitizenService : ITagCitizenService
{
    private readonly ITagCitizenRepository _tagCitizenRepository;
    private readonly ITagRepository _tagRepository;
    private readonly ICitizenRepository _citizenRepository;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;

    public TagCitizenService(
        ITagCitizenRepository tagCitizenRepository,
        ITagRepository tagRepository,
        ICitizenRepository citizenRepository,
        IUserContext userContext,
        IMapper mapper)
    {
        _tagCitizenRepository = tagCitizenRepository;
        _tagRepository = tagRepository;
        _citizenRepository = citizenRepository;
        _userContext = userContext;
        _mapper = mapper;
    }

    public async Task<TagCitizenResponseDTO> AddTagToCitizen(TagCitizenCreateDTO dto)
    {
        var ownerId = _userContext.OwnerId;

        var tag = await _tagRepository.GetByIdAsync(dto.TagId, ownerId);
        if (tag is null)
            throw new NotFoundException("TAG não encontrada");

        var citizen = await _citizenRepository.GetAsync(dto.CitizenId, ownerId);
        if (citizen is null)
            throw new NotFoundException("Eleitor não encontrado");

        var alreadyExists = await _tagCitizenRepository.GetByIdAsync(
            dto.TagId,
            dto.CitizenId,
            ownerId
        );

        if (alreadyExists is not null)
            throw new BadRequestException("Essa TAG já está associada a este eleitor");

        var tagCitizen = new TagCitizen
        {
            TagId = dto.TagId,
            CitizenId = dto.CitizenId,
            Reason = dto.Reason,
            CreatedAt = DateTime.UtcNow,
            Tag = tag,
            Citizen = citizen
        };

        await _tagCitizenRepository.AddAsync(tagCitizen);
        await _tagCitizenRepository.SaveChangesAsync();

        return _mapper.Map<TagCitizenResponseDTO>(tagCitizen);
    }

    public async Task<IEnumerable<TagCitizenResponseDTO>> GetAllByCitizenId(Guid citizenId)
    {
        var ownerId = _userContext.OwnerId;

        var citizen = await _citizenRepository.GetAsync(citizenId, ownerId);
        if (citizen is null)
            throw new NotFoundException("Eleitor não encontrado");

        var tags = await _tagCitizenRepository.GetAllByCitizenIdAsync(citizenId, ownerId);

        return _mapper.Map<IEnumerable<TagCitizenResponseDTO>>(tags);
    }

    public async Task<IEnumerable<TagCitizenResponseDTO>> GetAllByTagId(Guid tagId)
    {
        var ownerId = _userContext.OwnerId;

        var tag = await _tagRepository.GetByIdAsync(tagId, ownerId);
        if (tag is null)
            throw new NotFoundException("TAG não encontrada");

        var citizens = await _tagCitizenRepository.GetAllByTagIdAsync(tagId, ownerId);

        return _mapper.Map<IEnumerable<TagCitizenResponseDTO>>(citizens);
    }

    public async Task RemoveTagFromCitizen(Guid tagId, Guid citizenId)
    {
        var ownerId = _userContext.OwnerId;

        var tagCitizen = await _tagCitizenRepository.GetByIdAsync(
            tagId,
            citizenId,
            ownerId
        );

        if (tagCitizen is null)
            throw new NotFoundException("TAG não está associada a este eleitor");

        _tagCitizenRepository.Delete(tagCitizen);
        await _tagCitizenRepository.SaveChangesAsync();
    }
}
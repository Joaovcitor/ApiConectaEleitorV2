using AutoMapper;
using ConectaEleitor.Application.DTOs.CitizenDTOs;
using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.Interfaces;
using ConectaEleitor.Domain.Entities;
using ConectaEleitor.Domain.Enums;
using ConectaEleitor.Domain.Exceptions;

namespace ConectaEleitor.Application.Services;

public class CitizenService : ICitizenService
{
    private readonly ICitizenRepository _citizenRepository;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;

    public CitizenService(ICitizenRepository citizenRepository, IMapper mapper, IUserContext userContext)
    {
        _citizenRepository = citizenRepository;
        _mapper = mapper;
        _userContext = userContext;
    }

    public async Task<CitizenResponseDTO> Create(CitizenCreateDTO citizen)
    {
        var ownerId = _userContext.OwnerId;
        citizen.Cpf = NormalizeCpf(citizen.Cpf);
        if (!string.IsNullOrWhiteSpace(citizen.Cpf))
        {
            var citizenExist = await _citizenRepository.FindByCpf(citizen.Cpf);

            if (citizenExist)
            {
                throw new ConflictException(
                    "Eleitor cadastrado no sistema. Verifique se ele não pertence a outro vereador!");
            }
        }
        var citizenDto = _mapper.Map<Citizen>(citizen);
        citizenDto.OwnerId = ownerId;
        citizenDto.CreatedAt = DateTime.UtcNow;
        await _citizenRepository.AddAsync(citizenDto);
        await _citizenRepository.SaveChangesAsync();
        return _mapper.Map<CitizenResponseDTO>(citizenDto);
    }

    public async Task<CitizenResponseByIdDTO> GetById(Guid citizenId)
    {
        var ownerId = _userContext.OwnerId;
        var citizen = await _citizenRepository.GetAsync(citizenId, ownerId);
        if (citizen == null)
        {
            throw new NotFoundException();
        }
        return _mapper.Map<CitizenResponseByIdDTO>(citizen);
    }

    public async Task<PagedResult<CitizenResponseDTO>> GetAll(PaginationParams paginationParams)
    {
        var ownerId = _userContext.OwnerId;
        var citizens = await _citizenRepository.GetPagedAsync(paginationParams, ownerId);
        var data = _mapper.Map<IEnumerable<CitizenResponseDTO>>(citizens.Data);
        return new PagedResult<CitizenResponseDTO>(data, citizens.TotalCount, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public async Task<PagedResult<CitizenResponseDTO>> GetPagedCitizensLeaderAsync(PaginationParams paginationParams)
    {
        var ownerId = _userContext.OwnerId;
        var citizens = await _citizenRepository.GetPagedCitizensLeaderAsync(paginationParams, ownerId);
        var data = _mapper.Map<IEnumerable<CitizenResponseDTO>>(citizens.Data);
        return new PagedResult<CitizenResponseDTO>(data, citizens.TotalCount, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public async Task<CitizenResponseByIdDTO> Update(CitizenUpdateDTO citizenUpdateDto, Guid citizenId)
    {
        var ownerId = _userContext.OwnerId;
        var citizen = await _citizenRepository.GetAsync(citizenId, ownerId);
        if (citizen.OwnerId != ownerId)
        {
            throw new UnauthorizedException();
        }
        if (citizen is null)
        {
            throw new NotFoundException();
        }
        _mapper.Map(citizenUpdateDto, citizen);
        citizen.UpdatedAt = DateTime.UtcNow;
        _citizenRepository.Update(citizen);
        await _citizenRepository.SaveChangesAsync();
        return _mapper.Map<CitizenResponseByIdDTO>(citizen);
    }

    public async Task<string> DeleteById(Guid id)
    {
        var ownerId = _userContext.OwnerId;
        var citizen = await _citizenRepository.GetAsync(id, ownerId);
        if (citizen.OwnerId != ownerId)
        {
            throw new UnauthorizedException();
        }
        if (citizen is null)
        {
            throw new NotFoundException();
        }
        _citizenRepository.Delete(citizen);
        await _citizenRepository.SaveChangesAsync();
        return citizen.Type == CitizenType.Leader ? "Liderança deletada com sucesso" : "Eleitor deletado com sucesso!";
    }

    private static string? NormalizeCpf(string? cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return null;
        return new string(cpf.Where(char.IsDigit).ToArray());
    }
}
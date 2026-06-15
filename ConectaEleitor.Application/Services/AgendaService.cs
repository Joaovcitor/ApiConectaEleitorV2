using AutoMapper;
using ConectaEleitor.Application.DTOs.AgendasDTOs;
using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.Interfaces;
using ConectaEleitor.Domain.Entities;
using ConectaEleitor.Domain.Exceptions;

namespace ConectaEleitor.Application.Services;

public class AgendaService : IAgendaService
{
    private readonly IAgendaRepository _agendaRepository;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;

    public AgendaService(IAgendaRepository agendaRepository, IMapper mapper, IUserContext userContext)
    {
        _agendaRepository = agendaRepository;
        _mapper = mapper;
        _userContext = userContext;
    }

    public async Task<AgendaResponseDTO> CreateAsync(AgendaCreateDTO agendaCreateDTO)
    {
        var ownerId = _userContext.OwnerId;
        var agenda = _mapper.Map<Agenda>(agendaCreateDTO);
        agenda.OwnerId = ownerId;
        agenda.CreatedAt = DateTime.UtcNow;
        await  _agendaRepository.AddAsync(agenda);
        await _agendaRepository.SaveChangesAsync();
        return _mapper.Map<AgendaResponseDTO>(agenda);
    }

    public async Task<AgendaResponseByIdDTO> GetByIdAsync(Guid agendaId)
    {
        var ownerId = _userContext.OwnerId;
        var agenda = await  _agendaRepository.GetByIdAsync(agendaId, ownerId);
        if (agenda is null)
        {
            throw new NotFoundException();
        }
        return _mapper.Map<AgendaResponseByIdDTO>(agenda);
    }

    public async Task<PagedResult<AgendaResponseDTO>> GetAll(PaginationParams paginationParams)
    {
        var ownerId = _userContext.OwnerId;
        var agendas = await _agendaRepository.GetAllAsync(paginationParams, ownerId);
        var data = _mapper.Map<IEnumerable<AgendaResponseDTO>>(agendas.Data);
        return new PagedResult<AgendaResponseDTO>(data, agendas.TotalCount, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public async Task<AgendaResponseDTO> UpdateAsync(Guid agendaId, AgendaUpdateDTO agendaUpdateDTO)
    {
        var ownerId = _userContext.OwnerId;
        var agenda = await _agendaRepository.GetByIdAsync(agendaId, ownerId);
        if (agenda.OwnerId != ownerId)
        {
            throw new UnauthorizedException("Você não pode atualizar uma agenda que não lhe pertence!");
        }

        if (agenda is null)
        {
            throw new NotFoundException();
        }

        _mapper.Map(agendaUpdateDTO, agenda);
        _agendaRepository.UpdateAsync(agenda);
        await _agendaRepository.SaveChangesAsync();
        return  _mapper.Map<AgendaResponseDTO>(agenda);
    }

    public async Task DeleteAsync(Guid agendaId)
    {
        var ownerId = _userContext.OwnerId;
        var agenda = await _agendaRepository.GetByIdAsync(agendaId, ownerId);
        if (agenda.OwnerId != ownerId)
        {
            throw new UnauthorizedException("Você não pode deletar uma agenda que não lhe pertence!");
        }

        if (agenda is null)
        {
            throw new NotFoundException();
        }
        _agendaRepository.DeleteAsync(agenda);
        await _agendaRepository.SaveChangesAsync();
    }
}
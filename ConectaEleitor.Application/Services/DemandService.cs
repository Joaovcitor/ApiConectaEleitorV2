using AutoMapper;
using ConectaEleitor.Application.DTOs.DemandsDTOs;
using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.Interfaces;
using ConectaEleitor.Domain.Entities;
using ConectaEleitor.Domain.Enums;
using ConectaEleitor.Domain.Exceptions;

namespace ConectaEleitor.Application.Services;

public class DemandService : IDemandService
{
    private readonly IDemandRepository _demandRepository;
    private readonly ICitizenRepository _citizenRepository;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;

    public DemandService(IDemandRepository demandRepository, IMapper mapper, ICitizenRepository citizenRepository, IUserContext userContext)
    {
        _demandRepository = demandRepository;
        _mapper = mapper;
        _userContext = userContext;
        _citizenRepository = citizenRepository;
    }

    public async Task<DemandResponseDTO> Create(DemandCreateDTO demandCreateDTO)
    {
        var ownerId = _userContext.OwnerId;
        var demand = _mapper.Map<Demand>(demandCreateDTO);
        var citizenExist = await _citizenRepository.GetAsync(ownerId, demand.CitizenId);
        if (citizenExist != null)
        {
            throw new NotFoundException("Eleitor não encontrado!");
        }
        demand.OwnerId = ownerId;
        await _demandRepository.AddAsync(demand);
        await _demandRepository.SaveChangesAsync();
        return _mapper.Map<DemandResponseDTO>(demand);
    }

    public async Task<DemandResponseByIdDTO> GetById(Guid demandId)
    {
        var ownerId = _userContext.OwnerId;
        var demand = await _demandRepository.GetByIdAsync(demandId, ownerId);
        if (demand == null)
        {
            throw new NotFoundException("Demanda não encontrada!");
        }
        var response = _mapper.Map<DemandResponseByIdDTO>(demand);
        response.CitizenName = demand.Citizen.FullName;
        return response;
    }

    public async Task<PagedResult<DemandResponseDTO>> GetAll(PaginationParams paginationParams)
    {
        var ownerId =  _userContext.OwnerId;
        var demands = await _demandRepository.GetAllAsync(paginationParams, ownerId);
        var data = _mapper.Map<IEnumerable<DemandResponseDTO>>(demands.Data);
        return new PagedResult<DemandResponseDTO>(data, demands.TotalCount, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public async Task<PagedResult<DemandResponseDTO>> GetAllDemandsByCitizenId(PaginationParams paginationParams, Guid citizenId)
    {
        var ownerId = _userContext.OwnerId;
        var citizen = await _citizenRepository.GetAsync(citizenId, ownerId);
        if (citizen == null)
        {
            throw new NotFoundException("Eleitor ou liderança não existe");
        }
        var demands = await _demandRepository.GetAllDemandsByCitizenId(paginationParams, citizenId, ownerId);
        var data = _mapper.Map<IEnumerable<DemandResponseDTO>>(demands.Data);
        return new PagedResult<DemandResponseDTO>(data, demands.TotalCount, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public async Task<DemandResponseByIdDTO> Update(DemandUpdateDTO demandUpdateDTO, Guid demandId)
    {
        var ownerId = _userContext.OwnerId;
        var demand = await _demandRepository.GetByIdAsync(demandId, ownerId);
        if (demand is null)
        {
            throw new NotFoundException("Demanda não encontrada!");
        }

        _mapper.Map(demandUpdateDTO, demand);
        if (demand.Status == DemandStatus.Resolved)
        {
            demand.CompletedAt = DateTime.UtcNow;
        }
        _demandRepository.Update(demand);
        await _demandRepository.SaveChangesAsync();
        return _mapper.Map<DemandResponseByIdDTO>(demand);
    }

    public async Task Delete(Guid demandId)
    {
        var ownerId = _userContext.OwnerId;
        var demand = await _demandRepository.GetByIdAsync(ownerId, demandId);
        if (demand == null)
        {
            throw new NotFoundException("Demanda não encontrada!");
        }
        _demandRepository.Delete(demand);
        await _demandRepository.SaveChangesAsync();
    }
}
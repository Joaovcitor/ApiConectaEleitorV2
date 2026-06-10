using AutoMapper;
using ConectaEleitor.Application.DTOs.AppointmentsDTOs;
using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.Interfaces;
using ConectaEleitor.Domain.Entities;
using ConectaEleitor.Domain.Enums;
using ConectaEleitor.Domain.Exceptions;

namespace ConectaEleitor.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;

    public AppointmentService(IAppointmentRepository appointmentRepository, IMapper mapper, IUserContext userContext)
    {
        _appointmentRepository = appointmentRepository;
        _mapper = mapper;
        _userContext = userContext;
    }
    
    public async Task<AppointmentResponseDTO> Create(AppointmentCreateDTO appointmentCreateDTO)
    {
        var ownerId =  _userContext.OwnerId;
        var appointment = _mapper.Map<Appointment>(appointmentCreateDTO);
        appointment.OwnerId = ownerId;
        appointment.CreatedAt = DateTime.UtcNow;
        
        appointment.StartAt = DateTime.SpecifyKind(appointment.StartAt, DateTimeKind.Utc);
        appointment.EndAt = DateTime.SpecifyKind(appointment.EndAt, DateTimeKind.Utc);
        await _appointmentRepository.AddAsync(appointment);
        await _appointmentRepository.SaveChangesAsync();
        return _mapper.Map<AppointmentResponseDTO>(appointment);
    }

    public async Task<AppointmentResponseDTO> GetById(Guid apppointmentId)
    {
        var ownerId =  _userContext.OwnerId;
        var appointment = await _appointmentRepository.GetAsync(apppointmentId, ownerId);
        if (appointment == null)
        {
            throw new NotFoundException();
        }
        return _mapper.Map<AppointmentResponseDTO>(appointment);
    }

    public async Task<PagedResult<AppointmentResponseDTO>> GetAll(PaginationParams paginationParams)
    {
        var ownerId =  _userContext.OwnerId;
        var appointments = await _appointmentRepository.GetAllAsync(ownerId, paginationParams);
        var data = _mapper.Map<IEnumerable<AppointmentResponseDTO>>(appointments.Data);
        return new PagedResult<AppointmentResponseDTO>(data, appointments.TotalCount, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public async Task<AppointmentResponseByIdDTO> GetAppointmentById(Guid appointmentId)
    {
        var ownerId = _userContext.OwnerId;
        var appointment = await _appointmentRepository.GetAsync(appointmentId, ownerId);
        if (appointment == null)
        {
            throw new NotFoundException();
        }
        return _mapper.Map<AppointmentResponseByIdDTO>(appointment);
    }

    public async Task<AppointmentResponseDTO> Update(Guid appointmentId, AppointmentsUpdateDTO appointmentCreateDTO)
    {
        var ownerId =  _userContext.OwnerId;
        var appointment = await _appointmentRepository.GetAsync(appointmentId, ownerId);
        if (appointment.OwnerId != ownerId)
        {
            throw new UnauthorizedException("Você não pode atualizar compromissos que não são seus!");
        }

        if (appointment is null)
        {
            throw new NotFoundException();
        }

        if (appointment.Status == AppointmentStatus.Completed || appointment.Status == AppointmentStatus.Canceled)
        {
            throw new BadRequestException("Você não pode atualizar um compromisso que foi concluído ou cancelado!");
        }
        _mapper.Map(appointmentCreateDTO, appointment);
        _appointmentRepository.Update(appointment);
        await _appointmentRepository.SaveChangesAsync();
        return _mapper.Map<AppointmentResponseDTO>(appointment);
    }

    public async Task<string> Delete(Guid appointmentId)
    {
        var ownerId =  _userContext.OwnerId;
        var appointment = await _appointmentRepository.GetAsync(appointmentId, ownerId);
        if (appointment.OwnerId != ownerId)
        {
            throw new UnauthorizedException("Você não pode deletar compromissos que não são seus!");
        }
        _appointmentRepository.Delete(appointment);
        await _appointmentRepository.SaveChangesAsync();
        return "Compromisso deletado com sucesso!";
    }
}
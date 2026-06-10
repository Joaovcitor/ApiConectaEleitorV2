using ConectaEleitor.Application.DTOs.AppointmentsDTOs;

namespace ConectaEleitor.Application.DTOs.AgendasDTOs;

public class AgendaResponseByIdDTO
{
    public Guid AgendaId { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public ICollection<AppointmentResponseDTO> Appointments { get; set; } = [];

    public DateTime CreatedAt { get; set; }
}
using ConectaEleitor.Domain.Enums;

namespace ConectaEleitor.Application.DTOs.ReportsDTOs;

public class CitizensByTypeDTO
{
    public CitizenType Type { get; set; }
    public int Total { get; set; }
}
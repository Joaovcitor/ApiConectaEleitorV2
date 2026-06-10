using ConectaEleitor.Domain.Enums;

namespace ConectaEleitor.Application.DTOs.ReportsDTOs;

public class DemandsByStatusDTO
{
    public DemandStatus Status { get; set; }
    public int Total { get; set; }
}
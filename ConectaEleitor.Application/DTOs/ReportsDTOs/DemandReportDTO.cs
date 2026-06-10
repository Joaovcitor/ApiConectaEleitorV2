namespace ConectaEleitor.Application.DTOs.ReportsDTOs;

public class DemandReportDTO
{
    public int TotalDemands { get; set; }
    public int OpenDemands { get; set; }
    public int InProgressDemands { get; set; }
    public int CompletedDemands { get; set; }
    public int CanceledDemands { get; set; }

    public List<DemandsByStatusDTO> DemandsByStatus { get; set; } = [];
    public List<DemandsByMonthDTO> DemandsByMonth { get; set; } = [];
}
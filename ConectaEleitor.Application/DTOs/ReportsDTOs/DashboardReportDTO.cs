namespace ConectaEleitor.Application.DTOs.ReportsDTOs;

public class DashboardReportDTO
{
    public int TotalCitizens { get; set; }
    public int TotalActiveCitizens { get; set; }
    public int TotalInactiveCitizens { get; set; }

    public int TotalLeaders { get; set; }
    public int TotalVoters { get; set; }

    public DemandReportDTO Demands { get; set; } = new();
    public AppointmentReportDTO Appointments { get; set; } = new();

    public List<CitizensByTypeDTO> CitizensByType { get; set; } = [];
    public List<CitizensByNeighborhoodDTO> CitizensByNeighborhood { get; set; } = [];
    public List<CitizensByDistrictDTO> CitizensByDistrict { get; set; } = [];
    public List<LeaderRankingDTO> TopLeaders { get; set; } = [];
}
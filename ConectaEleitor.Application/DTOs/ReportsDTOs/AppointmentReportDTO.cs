namespace ConectaEleitor.Application.DTOs.ReportsDTOs;

public class AppointmentReportDTO
{
    public int TotalAppointments { get; set; }
    public int ScheduledAppointments { get; set; }
    public int CompletedAppointments { get; set; }
    public int CanceledAppointments { get; set; }

    public List<AppointmentsByStatusDTO> AppointmentsByStatus { get; set; } = [];
    public List<AppointmentsByMonthDTO> AppointmentsByMonth { get; set; } = [];
}
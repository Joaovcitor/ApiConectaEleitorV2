using ConectaEleitor.Application.DTOs.ReportsDTOs;
using ConectaEleitor.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConectaEleitor.API.Controllers;

[ApiController]
[Route("api/reports")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("dashboard")]
    [ProducesResponseType(typeof(DashboardReportDTO), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDashboard(
        [FromQuery] ReportBaseFilterDTO filter)
    {
        var result = await _reportService.GetDashboardAsync(filter);

        return Ok(result);
    }

    [HttpGet("demands")]
    [ProducesResponseType(typeof(DemandReportDTO), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDemandsReport(
        [FromQuery] DemandReportFilterDTO filter)
    {
        var result = await _reportService.GetDemandsReportAsync(filter);

        return Ok(result);
    }

    [HttpGet("appointments")]
    [ProducesResponseType(typeof(AppointmentReportDTO), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAppointmentsReport(
        [FromQuery] AppointmentReportFilterDTO filter)
    {
        var result = await _reportService.GetAppointmentsReportAsync(filter);

        return Ok(result);
    }

    [HttpGet("top-leaders")]
    [ProducesResponseType(typeof(List<LeaderRankingDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTopLeaders(
        [FromQuery] ReportBaseFilterDTO filter)
    {
        var result = await _reportService.GetTopLeadersAsync(filter);

        return Ok(result);
    }
}
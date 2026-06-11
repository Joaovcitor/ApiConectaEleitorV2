using ConectaEleitor.Application.DTOs.PlanUsages;
using ConectaEleitor.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConectaEleitor.API.Controllers;

[Route("api/plan-usages")]
[ApiController]
[Authorize]
public class PlanUsagesController : ControllerBase
{
    private readonly IPlanUsageService _planUsageService;

    public PlanUsagesController(IPlanUsageService planUsageService)
    {
        _planUsageService = planUsageService;
    }

    [HttpGet("current")]
    public async Task<IActionResult> GetCurrent()
    {
        return Ok(await _planUsageService.GetCurrentUsageAsync());
    }

    [HttpGet("{year:int}/{month:int}")]
    public async Task<IActionResult> GetByPeriod(int year, int month)
    {
        return Ok(await _planUsageService.GetByPeriodAsync(year, month));
    }

    [HttpPut("current")]
    public async Task<IActionResult> UpdateCurrent([FromBody] PlanUsageUpdateDTO dto)
    {
        return Ok(await _planUsageService.UpdateUsageAsync(dto));
    }
}

using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.DTOs.Plans;
using ConectaEleitor.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConectaEleitor.API.Controllers;

[Route("api/plans")]
[ApiController]
[Authorize]
public class PlansController : ControllerBase
{
    private readonly IPlanService _planService;

    public PlansController(IPlanService planService)
    {
        _planService = planService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] PaginationParams paginationParams, [FromQuery] bool onlyActive = true)
    {
        return Ok(await _planService.GetPagedAsync(paginationParams, onlyActive));
    }

    [HttpGet("public")]
    public async Task<IActionResult> GetPublic([FromQuery] PaginationParams paginationParams)
    {
        return Ok(await _planService.GetPublicPagedAsync(paginationParams));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _planService.GetByIdAsync(id));
    }

    [HttpGet("slug/{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        return Ok(await _planService.GetBySlugAsync(slug));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] PlanCreateDTO dto)
    {
        return Ok(await _planService.CreateAsync(dto));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] PlanUpdateDTO dto)
    {
        return Ok(await _planService.UpdateAsync(id, dto));
    }

    [HttpPatch("{id:guid}/activate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Activate(Guid id)
    {
        return Ok(await _planService.ActivateAsync(id));
    }

    [HttpPatch("{id:guid}/deactivate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        return Ok(await _planService.DeactivateAsync(id));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _planService.DeleteAsync(id);
        return NoContent();
    }
}

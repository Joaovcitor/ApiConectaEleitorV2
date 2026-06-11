using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.DTOs.Subscriptions;
using ConectaEleitor.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConectaEleitor.API.Controllers;

[Route("api/subscriptions")]
[ApiController]
[Authorize]
public class OwnerSubscriptionsController : ControllerBase
{
    private readonly IOwnerSubscriptionService _subscriptionService;

    public OwnerSubscriptionsController(IOwnerSubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        return Ok(await _subscriptionService.GetActiveByOwnerAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _subscriptionService.GetByIdAsync(id));
    }

    [HttpGet("owner")]
    public async Task<IActionResult> GetByOwner([FromQuery] PaginationParams paginationParams)
    {
        return Ok(await _subscriptionService.GetPagedByOwnerAsync(paginationParams));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] OwnerSubscriptionCreateDTO dto)
    {
        return Ok(await _subscriptionService.CreateAsync(dto));
    }

    [HttpPatch("{id:guid}/change-plan")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangePlan(Guid id, [FromBody] ChangePlanDTO dto)
    {
        return Ok(await _subscriptionService.ChangePlanAsync(id, dto));
    }

    [HttpPatch("{id:guid}/cancel")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Cancel(Guid id, [FromBody] CancelSubscriptionDTO dto)
    {
        return Ok(await _subscriptionService.CancelAsync(id, dto));
    }

    [HttpPatch("{id:guid}/suspend")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Suspend(Guid id, [FromBody] OwnerSubscriptionUpdateStatusDTO dto)
    {
        return Ok(await _subscriptionService.SuspendAsync(id, dto));
    }

    [HttpPatch("{id:guid}/reactivate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Reactivate(Guid id)
    {
        return Ok(await _subscriptionService.ReactivateAsync(id));
    }
}

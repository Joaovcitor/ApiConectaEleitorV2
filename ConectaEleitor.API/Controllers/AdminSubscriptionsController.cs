using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.DTOs.Subscriptions;
using ConectaEleitor.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConectaEleitor.API.Controllers;

[Route("api/admin/subscriptions")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminSubscriptionsController : ControllerBase
{
    private readonly IOwnerSubscriptionService _subscriptionService;

    public AdminSubscriptionsController(IOwnerSubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] PaginationParams paginationParams)
    {
        return Ok(await _subscriptionService.AdminGetPagedAsync(paginationParams));
    }

    [HttpGet("owner/{ownerId:guid}")]
    public async Task<IActionResult> GetByOwner(Guid ownerId, [FromQuery] PaginationParams paginationParams)
    {
        return Ok(await _subscriptionService.AdminGetByOwnerAsync(ownerId, paginationParams));
    }

    [HttpPost("assign-plan")]
    public async Task<IActionResult> AssignPlan([FromBody] ManualAssignPlanDTO dto)
    {
        return Ok(await _subscriptionService.AdminAssignPlanAsync(dto));
    }

    [HttpPatch("change-plan")]
    public async Task<IActionResult> ChangePlan([FromBody] AdminChangeOwnerPlanDTO dto)
    {
        return Ok(await _subscriptionService.AdminChangeOwnerPlanAsync(dto));
    }

    [HttpPatch("cancel")]
    public async Task<IActionResult> Cancel([FromBody] AdminCancelSubscriptionDTO dto)
    {
        return Ok(await _subscriptionService.AdminCancelOwnerSubscriptionAsync(dto));
    }

    [HttpPatch("suspend")]
    public async Task<IActionResult> Suspend([FromBody] AdminSuspendSubscriptionDTO dto)
    {
        return Ok(await _subscriptionService.AdminSuspendOwnerSubscriptionAsync(dto));
    }

    [HttpPatch("reactivate")]
    public async Task<IActionResult> Reactivate([FromBody] AdminReactivateSubscriptionDTO dto)
    {
        return Ok(await _subscriptionService.AdminReactivateOwnerSubscriptionAsync(dto));
    }
}

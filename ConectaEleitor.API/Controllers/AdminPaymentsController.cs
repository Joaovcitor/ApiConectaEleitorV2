using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.DTOs.Payments;
using ConectaEleitor.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConectaEleitor.API.Controllers;

[Route("api/admin/payments")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminPaymentsController : ControllerBase
{
    private readonly IPaymentTransactionService _paymentService;

    public AdminPaymentsController(IPaymentTransactionService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet("owner/{ownerId:guid}")]
    public async Task<IActionResult> GetByOwner(
        Guid ownerId,
        [FromQuery] PaymentTransactionFilterDTO filter,
        [FromQuery] PaginationParams paginationParams)
    {
        return Ok(await _paymentService.AdminGetPagedByOwnerAsync(ownerId, filter, paginationParams));
    }

    [HttpPatch("{id:guid}/paid")]
    public async Task<IActionResult> MarkAsPaid(Guid id)
    {
        return Ok(await _paymentService.MarkAsPaidAsync(id));
    }

    [HttpPatch("{id:guid}/failed")]
    public async Task<IActionResult> MarkAsFailed(Guid id, [FromBody] PaymentTransactionUpdateStatusDTO dto)
    {
        return Ok(await _paymentService.MarkAsFailedAsync(id, dto));
    }

    [HttpPatch("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        return Ok(await _paymentService.CancelAsync(id));
    }

    [HttpPatch("{id:guid}/refund")]
    public async Task<IActionResult> Refund(Guid id)
    {
        return Ok(await _paymentService.RefundAsync(id));
    }
}

using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.DTOs.Payments;
using ConectaEleitor.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConectaEleitor.API.Controllers;

[Route("api/payments")]
[ApiController]
[Authorize]
public class PaymentTransactionsController : ControllerBase
{
    private readonly IPaymentTransactionService _paymentService;

    public PaymentTransactionsController(IPaymentTransactionService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaged(
        [FromQuery] PaymentTransactionFilterDTO filter,
        [FromQuery] PaginationParams paginationParams)
    {
        return Ok(await _paymentService.GetPagedByOwnerAsync(filter, paginationParams));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _paymentService.GetByIdAsync(id));
    }

    [HttpGet("subscription/{subscriptionId:guid}")]
    public async Task<IActionResult> GetBySubscription(Guid subscriptionId, [FromQuery] PaginationParams paginationParams)
    {
        return Ok(await _paymentService.GetPagedBySubscriptionAsync(subscriptionId, paginationParams));
    }

    [HttpGet("pending")]
    public async Task<IActionResult> GetPending()
    {
        return Ok(await _paymentService.GetPendingByOwnerAsync());
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] PaymentTransactionCreateDTO dto)
    {
        return Ok(await _paymentService.CreateAsync(dto));
    }

    [HttpPatch("{id:guid}/paid")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> MarkAsPaid(Guid id)
    {
        return Ok(await _paymentService.MarkAsPaidAsync(id));
    }

    [HttpPatch("{id:guid}/failed")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> MarkAsFailed(Guid id, [FromBody] PaymentTransactionUpdateStatusDTO dto)
    {
        return Ok(await _paymentService.MarkAsFailedAsync(id, dto));
    }

    [HttpPatch("{id:guid}/cancel")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        return Ok(await _paymentService.CancelAsync(id));
    }

    [HttpPatch("{id:guid}/refund")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Refund(Guid id)
    {
        return Ok(await _paymentService.RefundAsync(id));
    }
}

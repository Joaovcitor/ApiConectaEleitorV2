using AutoMapper;
using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.DTOs.Payments;
using ConectaEleitor.Application.Interfaces;
using ConectaEleitor.Domain.Entities;
using ConectaEleitor.Domain.Enums;
using ConectaEleitor.Domain.Exceptions;

namespace ConectaEleitor.Application.Services;

public class PaymentTransactionService : IPaymentTransactionService
{
    private readonly IPaymentTransactionRepository _paymentRepository;
    private readonly IOwnerSubscriptionRepository _subscriptionRepository;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;

    public PaymentTransactionService(
        IPaymentTransactionRepository paymentRepository,
        IOwnerSubscriptionRepository subscriptionRepository,
        IUserContext userContext,
        IMapper mapper)
    {
        _paymentRepository = paymentRepository;
        _subscriptionRepository = subscriptionRepository;
        _userContext = userContext;
        _mapper = mapper;
    }

    public async Task<PaymentTransactionResponseDTO> CreateAsync(PaymentTransactionCreateDTO dto)
    {
        EnsureAdmin();

        if (dto.Amount <= 0)
            throw new BadRequestException("O valor do pagamento deve ser maior que zero.");

        if (dto.Status is not (PaymentStatus.Pending or PaymentStatus.WaitingPayment))
            throw new BadRequestException("O status inicial deve ser Pending ou WaitingPayment.");

        var subscription = await GetOwnedSubscriptionOrThrow(dto.OwnerSubscriptionId);
        var payment = _mapper.Map<PaymentTransaction>(dto);
        payment.PaymentTransactionId = Guid.NewGuid();
        payment.OwnerId = _userContext.OwnerId;
        payment.PlanId = subscription.PlanId;
        payment.CreatedAt = DateTime.UtcNow;
        payment.Currency = string.IsNullOrWhiteSpace(payment.Currency) ? "BRL" : payment.Currency.ToUpperInvariant();

        await _paymentRepository.AddAsync(payment);
        await _paymentRepository.SaveChangesAsync();

        payment.Plan = subscription.Plan;
        payment.OwnerSubscription = subscription;
        return _mapper.Map<PaymentTransactionResponseDTO>(payment);
    }

    public async Task<PaymentTransactionResponseDTO> GetByIdAsync(Guid paymentId)
    {
        return _mapper.Map<PaymentTransactionResponseDTO>(await GetOwnedPaymentOrThrow(paymentId));
    }

    public async Task<PagedResult<PaymentTransactionResponseDTO>> GetPagedByOwnerAsync(
        PaymentTransactionFilterDTO filter,
        PaginationParams paginationParams)
    {
        var payments = await _paymentRepository.GetPagedByOwnerIdAsync(
            _userContext.OwnerId,
            filter,
            paginationParams);

        var data = _mapper.Map<IEnumerable<PaymentTransactionResponseDTO>>(payments.Data);
        return new PagedResult<PaymentTransactionResponseDTO>(data, payments.TotalCount, payments.PageNumber, payments.PageSize);
    }

    public async Task<PagedResult<PaymentTransactionResponseDTO>> AdminGetPagedByOwnerAsync(
        Guid ownerId,
        PaymentTransactionFilterDTO filter,
        PaginationParams paginationParams)
    {
        EnsureAdmin();

        var payments = await _paymentRepository.GetPagedByOwnerIdAsync(
            ownerId,
            filter,
            paginationParams);

        var data = _mapper.Map<IEnumerable<PaymentTransactionResponseDTO>>(payments.Data);
        return new PagedResult<PaymentTransactionResponseDTO>(data, payments.TotalCount, payments.PageNumber, payments.PageSize);
    }

    public async Task<PagedResult<PaymentTransactionResponseDTO>> GetPagedBySubscriptionAsync(
        Guid subscriptionId,
        PaginationParams paginationParams)
    {
        await GetOwnedSubscriptionOrThrow(subscriptionId);
        var payments = await _paymentRepository.GetPagedBySubscriptionIdAsync(subscriptionId, paginationParams);
        var data = _mapper.Map<IEnumerable<PaymentTransactionResponseDTO>>(payments.Data);
        return new PagedResult<PaymentTransactionResponseDTO>(data, payments.TotalCount, payments.PageNumber, payments.PageSize);
    }

    public async Task<IEnumerable<PaymentTransactionResponseDTO>> GetPendingByOwnerAsync()
    {
        var payments = await _paymentRepository.GetPendingByOwnerIdAsync(_userContext.OwnerId);
        return _mapper.Map<IEnumerable<PaymentTransactionResponseDTO>>(payments);
    }

    public async Task<PaymentTransactionResponseDTO> MarkAsPaidAsync(Guid paymentId)
    {
        EnsureAdmin();

        var payment = await GetPaymentOrThrow(paymentId);
        var now = DateTime.UtcNow;

        payment.Status = PaymentStatus.Paid;
        payment.PaidAt = now;
        payment.UpdatedAt = now;

        var subscription = payment.OwnerSubscription;
        if (subscription.Status is SubscriptionStatus.PastDue or SubscriptionStatus.Suspended or SubscriptionStatus.Trial)
            subscription.Status = SubscriptionStatus.Active;

        subscription.CurrentPeriodStart = now;
        subscription.CurrentPeriodEnd = payment.BillingCycle == BillingCycle.Yearly ? now.AddYears(1) : now.AddMonths(1);
        subscription.UpdatedAt = now;

        _paymentRepository.Update(payment);
        await _paymentRepository.SaveChangesAsync();

        return _mapper.Map<PaymentTransactionResponseDTO>(payment);
    }

    public async Task<PaymentTransactionResponseDTO> MarkAsFailedAsync(Guid paymentId, PaymentTransactionUpdateStatusDTO dto)
    {
        EnsureAdmin();

        var payment = await GetPaymentOrThrow(paymentId);
        payment.Status = PaymentStatus.Failed;
        payment.FailedAt = DateTime.UtcNow;
        payment.FailureReason = dto.FailureReason;
        payment.UpdatedAt = DateTime.UtcNow;

        _paymentRepository.Update(payment);
        await _paymentRepository.SaveChangesAsync();

        return _mapper.Map<PaymentTransactionResponseDTO>(payment);
    }

    public async Task<PaymentTransactionResponseDTO> CancelAsync(Guid paymentId)
    {
        EnsureAdmin();

        var payment = await GetPaymentOrThrow(paymentId);
        payment.Status = PaymentStatus.Canceled;
        payment.UpdatedAt = DateTime.UtcNow;

        _paymentRepository.Update(payment);
        await _paymentRepository.SaveChangesAsync();

        return _mapper.Map<PaymentTransactionResponseDTO>(payment);
    }

    public async Task<PaymentTransactionResponseDTO> RefundAsync(Guid paymentId)
    {
        EnsureAdmin();

        var payment = await GetPaymentOrThrow(paymentId);
        payment.Status = PaymentStatus.Refunded;
        payment.RefundedAt = DateTime.UtcNow;
        payment.UpdatedAt = DateTime.UtcNow;

        _paymentRepository.Update(payment);
        await _paymentRepository.SaveChangesAsync();

        return _mapper.Map<PaymentTransactionResponseDTO>(payment);
    }

    private async Task<OwnerSubscription> GetOwnedSubscriptionOrThrow(Guid subscriptionId)
    {
        var subscription = await _subscriptionRepository.GetByIdAsync(subscriptionId);
        if (subscription is null)
            throw new NotFoundException("Assinatura não encontrada.");

        if (subscription.OwnerId != _userContext.OwnerId)
            throw new UnauthorizedException();

        return subscription;
    }

    private async Task<PaymentTransaction> GetOwnedPaymentOrThrow(Guid paymentId)
    {
        var payment = await _paymentRepository.GetByIdAsync(paymentId);
        if (payment is null)
            throw new NotFoundException("Pagamento não encontrado.");

        if (payment.OwnerId != _userContext.OwnerId)
            throw new UnauthorizedException();

        return payment;
    }

    private async Task<PaymentTransaction> GetPaymentOrThrow(Guid paymentId)
    {
        var payment = await _paymentRepository.GetByIdAsync(paymentId);
        if (payment is null)
            throw new NotFoundException("Pagamento não encontrado.");

        return payment;
    }

    private void EnsureAdmin()
    {
        if (!_userContext.IsAdmin && !_userContext.Roles.Contains("Admin"))
            throw new UnauthorizedException("Apenas administradores podem executar esta operação.");
    }
}

namespace ConectaEleitor.Domain.Enums;

public enum PaymentStatus
{
    Pending = 1,
    WaitingPayment = 2,
    Paid = 3,
    Failed = 4,
    Canceled = 5,
    Refunded = 6,
    Expired = 7,
    Chargeback = 8
}
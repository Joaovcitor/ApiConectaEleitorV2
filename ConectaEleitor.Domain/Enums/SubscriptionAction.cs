namespace ConectaEleitor.Domain.Entities;

public enum SubscriptionAction
{
    Created = 1,
    PlanChanged = 2,
    Renewed = 3,
    Canceled = 4,
    Suspended = 5,
    Reactivated = 6,
    Expired = 7
}
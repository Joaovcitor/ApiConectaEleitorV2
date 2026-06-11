using AutoMapper;
using ConectaEleitor.Application.DTOs.Payments;
using ConectaEleitor.Application.DTOs.Plans;
using ConectaEleitor.Application.DTOs.PlanUsages;
using ConectaEleitor.Application.DTOs.Subscriptions;
using ConectaEleitor.Domain.Entities;

namespace ConectaEleitor.Application.Mappings;

public class PlanBillingMappingProfile : Profile
{
    public PlanBillingMappingProfile()
    {
        CreateMap<Plan, PlanResponseDTO>();
        CreateMap<PlanCreateDTO, Plan>();
        CreateMap<PlanUpdateDTO, Plan>();
        CreateMap<PlanFeature, PlanFeatureResponseDTO>();
        CreateMap<PlanFeatureCreateDTO, PlanFeature>();
        CreateMap<PlanLimit, PlanLimitResponseDTO>();
        CreateMap<PlanLimitCreateDTO, PlanLimit>();

        CreateMap<OwnerSubscription, OwnerSubscriptionResponseDTO>()
            .ForMember(dest => dest.PlanName,
                opt => opt.MapFrom(src => src.Plan.Name))
            .ForMember(dest => dest.PlanSlug,
                opt => opt.MapFrom(src => src.Plan.Slug));
        CreateMap<SubscriptionHistory, SubscriptionHistoryResponseDTO>();

        CreateMap<PaymentTransaction, PaymentTransactionResponseDTO>();
        CreateMap<PaymentTransactionCreateDTO, PaymentTransaction>()
            .ForMember(dest => dest.Currency,
                opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Currency) ? "BRL" : src.Currency));

        CreateMap<PlanUsage, PlanUsageResponseDTO>();
        CreateMap<PlanUsageUpdateDTO, PlanUsage>();
    }
}

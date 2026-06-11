using AutoMapper;
using ConectaEleitor.Application.DTOs.AssemblymanPostsDTOs;
using ConectaEleitor.Domain.Entities;

namespace ConectaEleitor.Application.Mappings;

public class AssemblymanPostMappingProfile : Profile
{
    public AssemblymanPostMappingProfile()
    {
        CreateMap<AssemblymanPostCreateDTO, AssemblymanPost>();

        CreateMap<AssemblymanPostUpdateDTO, AssemblymanPost>()
            .ForMember(dest => dest.AssemblymanPostId, opt => opt.Ignore())
            .ForMember(dest => dest.OwnerId, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ViewsCount, opt => opt.Ignore())
            .ForMember(dest => dest.LikesCount, opt => opt.Ignore())
            .ForMember(dest => dest.CommentsCount, opt => opt.Ignore())
            .ForMember(dest => dest.Comments, opt => opt.Ignore())
            .ForMember(dest => dest.Likes, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore());

        CreateMap<AssemblymanPost, AssemblymanPostResponseDTO>()
            .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));

        CreateMap<AssemblymanPostCommentCreateDTO, AssemblymanPostComment>();

        CreateMap<AssemblymanPostComment, AssemblymanPostCommentResponseDTO>();

        CreateMap<AssemblymanPostLike, AssemblymanPostLikeResponseDTO>();

        CreateMap<AssemblymanPostCategoryCreateDTO, AssemblymanPostCategory>();

        CreateMap<AssemblymanPostCategoryUpdateDTO, AssemblymanPostCategory>()
            .ForMember(dest => dest.AssemblymanPostCategoryId, opt => opt.Ignore())
            .ForMember(dest => dest.OwnerId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Posts, opt => opt.Ignore());

        CreateMap<AssemblymanPostCategory, AssemblymanPostCategoryResponseDTO>();
    }
}
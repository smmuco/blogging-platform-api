using AutoMapper;
using BloggingPlatform.Application.DTOs.Post;
using BloggingPlatform.Application.DTOs.User;
using PersonalBloggingPlatform.Domain.Entities;

namespace BloggingPlatform.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Post mappings
            CreateMap<Post, PostResponse>();
            CreateMap<CreatePostRequest, Post>();
            CreateMap<UpdatePostRequest, Post>();
            CreateMap<UpdatePostRequest, User>()
    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            // User mappings
            CreateMap<User, UserResponse>();
            CreateMap<RegisterUserRequest, User>();
        }
    }
}

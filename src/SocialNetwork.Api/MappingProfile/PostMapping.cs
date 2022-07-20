using AutoMapper;
using SocialNetwork.Api.Contracts.Posts.Responses;
using SocialNetwork.Domain.Aggregates.PostAggregate;

namespace SocialNetwork.Api.MappingProfile
{
    public class PostMapping : Profile
    {
        public PostMapping()
        {
            CreateMap<Post, PostResponse>();
            CreateMap<PostComment, PostCommentResponse>();
        }
    }
}

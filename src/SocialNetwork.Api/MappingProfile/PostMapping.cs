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
            CreateMap<PostInteraction, PostInteractionResponse>()
                .ForMember(dest 
                    => dest.Type, opt 
                    => opt.MapFrom(src
                    => src.Interaction.ToString()))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src=> src.UserProfile));

        }
    }
}

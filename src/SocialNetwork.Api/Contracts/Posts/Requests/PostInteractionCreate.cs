using SocialNetwork.Domain.Aggregates.PostAggregate;

namespace SocialNetwork.Api.Contracts.Posts.Requests
{
    public class PostInteractionCreate
    {
        [Required]
        public InteractionType Type { get; set; }
    }
}

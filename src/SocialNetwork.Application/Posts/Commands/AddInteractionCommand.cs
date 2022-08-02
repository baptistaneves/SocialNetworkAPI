using MediatR;
using SocialNetwork.Application.Models;
using SocialNetwork.Domain.Aggregates.PostAggregate;

namespace SocialNetwork.Application.Posts.Commands
{
    public class AddInteractionCommand : IRequest<OperationResult<PostInteraction>>
    {
        public Guid PostId { get; set; }
        public Guid UserProfileId { get; set; }
        public InteractionType Type { get; set; }
    }
}

using MediatR;
using SocialNetwork.Application.Models;
using SocialNetwork.Domain.Aggregates.PostAggregate;

namespace SocialNetwork.Application.Posts.Commands
{
    public class CreatePostCommand : IRequest<OperationResult<Post>>
    {
        public Guid UserProfileId { get; set; }
        public string TextContent { get; set; }
    }
}

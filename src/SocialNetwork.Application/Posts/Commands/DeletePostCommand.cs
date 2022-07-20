using MediatR;
using SocialNetwork.Application.Models;
using SocialNetwork.Domain.Aggregates.PostAggregate;

namespace SocialNetwork.Application.Posts.Commands
{
    public class DeletePostCommand : IRequest<OperationResult<Post>>
    {
        public Guid PostId { get; set; }
    }
}

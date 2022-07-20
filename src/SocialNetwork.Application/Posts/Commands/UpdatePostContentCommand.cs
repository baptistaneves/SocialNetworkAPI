using MediatR;
using SocialNetwork.Application.Models;
using SocialNetwork.Domain.Aggregates.PostAggregate;

namespace SocialNetwork.Application.Posts.Commands
{
    public class UpdatePostContentCommand : IRequest<OperationResult<Post>>
    {
        public string NewText { get; set; }
        public Guid PostId { get; set; }
    }
}

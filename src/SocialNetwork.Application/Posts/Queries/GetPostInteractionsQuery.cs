using MediatR;
using SocialNetwork.Application.Models;
using SocialNetwork.Domain.Aggregates.PostAggregate;

namespace SocialNetwork.Application.Posts.Queries
{
    public class GetPostInteractionsQuery : IRequest<OperationResult<List<PostInteraction>>>
    {
        public Guid PostId { get; set; }
    }
}

using MediatR;
using SocialNetwork.Application.Models;
using SocialNetwork.Domain.Aggregates.PostAggregate;

namespace SocialNetwork.Application.Posts.Queries
{
    public class GetPostCommentsQuery : IRequest<OperationResult<List<PostComment>>>
    {
        public Guid PostId { get; set; }
    }
}

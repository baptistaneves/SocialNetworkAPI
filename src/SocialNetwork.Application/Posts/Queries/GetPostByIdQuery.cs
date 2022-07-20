using MediatR;
using SocialNetwork.Application.Models;
using SocialNetwork.Domain.Aggregates.PostAggregate;

namespace SocialNetwork.Application.Posts.Queries
{
    public class GetPostByIdQuery : IRequest<OperationResult<Post>>
    {
        public Guid PostId { get; set; }
    }
}

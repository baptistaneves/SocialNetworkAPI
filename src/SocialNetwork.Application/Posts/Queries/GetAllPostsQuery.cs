using MediatR;
using SocialNetwork.Application.Models;
using SocialNetwork.Domain.Aggregates.PostAggregate;

namespace SocialNetwork.Application.Posts.Queries
{
    public class GetAllPostsQuery : IRequest<OperationResult<List<Post>>>
    {
    }
}

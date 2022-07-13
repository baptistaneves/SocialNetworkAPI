using MediatR;
using SocialNetwork.Application.Models;
using SocialNetwork.Domain.Aggregates.UserProfileAggregate;

namespace SocialNetwork.Application.UserProfiles.Queries
{
    public class GetAllUserProfileQuery : IRequest<OperationResult<IEnumerable<UserProfile>>>
    {
    }
}

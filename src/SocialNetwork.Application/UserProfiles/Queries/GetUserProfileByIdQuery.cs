using MediatR;
using SocialNetwork.Application.Models;
using SocialNetwork.Domain.Aggregates.UserProfileAggregate;

namespace SocialNetwork.Application.UserProfiles.Queries
{
    public class GetUserProfileByIdQuery : IRequest<OperationResult<UserProfile>>
    {
        public Guid UserProfileId { get; set; }
    }
}

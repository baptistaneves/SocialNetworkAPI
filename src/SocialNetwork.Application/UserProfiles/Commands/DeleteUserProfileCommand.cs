using MediatR;
using SocialNetwork.Application.Models;
using SocialNetwork.Domain.Aggregates.UserProfileAggregate;

namespace SocialNetwork.Application.UserProfiles.Commands
{
    public class DeleteUserProfileCommand : IRequest<OperationResult<UserProfile>>
    {
        public Guid UserProfileId { get; set; }
    }
}

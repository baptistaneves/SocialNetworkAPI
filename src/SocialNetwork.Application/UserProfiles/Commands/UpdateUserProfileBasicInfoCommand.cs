using MediatR;
using SocialNetwork.Application.Models;
using SocialNetwork.Domain.Aggregates.UserProfileAggregate;

namespace SocialNetwork.Application.UserProfiles.Commands
{
    public class UpdateUserProfileBasicInfoCommand : IRequest<OperationResult<UserProfile>>
    {
        public Guid UserProfileId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CurrentCity { get; set; }
    }
}

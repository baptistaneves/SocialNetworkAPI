using MediatR;
using SocialNetwork.Application.Models;
using SocialNetwork.Application.UserProfiles.Commands;
using SocialNetwork.Dal.Context;
using SocialNetwork.Domain.Aggregates.UserProfileAggregate;

namespace SocialNetwork.Application.UserProfiles.CommandHandlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, OperationResult<UserProfile>>
    {
        private readonly DataContext _context;

        public CreateUserCommandHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<UserProfile>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<UserProfile>();

            var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName,
                request.EmailAddress, request.Phone, request.DateOfBirth, request.CurrentCity);

            var userProfile = UserProfile.CreateUserProfile(Guid.NewGuid().ToString(), basicInfo);

            _context.UserProfiles.Add(userProfile);
            await _context.SaveChangesAsync();

            result.Payload = userProfile;

            return result;
        }
    }
}

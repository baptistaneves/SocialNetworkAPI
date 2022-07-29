using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Application.Enums;
using SocialNetwork.Application.Models;
using SocialNetwork.Application.UserProfiles.Commands;
using SocialNetwork.Dal.Context;
using SocialNetwork.Domain.Aggregates.UserProfileAggregate;
using SocialNetwork.Domain.Exceptions;

namespace SocialNetwork.Application.UserProfiles.CommandHandlers
{
    internal class UpdateUserProfileBasicInfoCommandHandler : IRequestHandler<UpdateUserProfileBasicInfoCommand, OperationResult<UserProfile>>
    {
        private readonly DataContext _context;

        public UpdateUserProfileBasicInfoCommandHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<UserProfile>> Handle(UpdateUserProfileBasicInfoCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<UserProfile>();

            try
            {
                var userProfile = await _context.UserProfiles
                            .FirstOrDefaultAsync(up => up.UserProfileId == request.UserProfileId, cancellationToken);

                if(userProfile is null)
                {
                    result.AddError(ErrorCode.NotFound,
                        string.Format(UserProfileErrorMessages.UserProfileNotFound, request.UserProfileId));
                    return result;
                }

                var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName,
                    request.EmailAddress, request.Phone, request.DateOfBirth, request.CurrentCity);

                userProfile.UpdateBasicInfo(basicInfo);

                _context.UserProfiles.Update(userProfile);

                await _context.SaveChangesAsync(cancellationToken);

                result.Payload = userProfile;
            }

            catch (UserProfileNotValidException ex)
            {
                ex.ValidationErrors.ForEach(error => result.AddError(ErrorCode.ValidationError, $"{error}"));
            }

            catch (Exception ex)
            {
                result.AddUnknownError($"{ex.Message}");
            }

            return result;
        }
    }
}

using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Application.Enums;
using SocialNetwork.Application.Models;
using SocialNetwork.Application.UserProfiles.Commands;
using SocialNetwork.Dal.Context;
using SocialNetwork.Domain.Aggregates.UserProfileAggregate;

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
                            .FirstOrDefaultAsync(up => up.UserProfileId == request.UserProfileId);

                if(userProfile is null)
                {
                    result.IsError = true;
                    var error = new Error { Code = ErrorCode.NotFound, Message = $"No User Profile found with ID {request.UserProfileId}" };
                    result.Errors.Add(error);
                    return result;
                }

                var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName,
                    request.EmailAddress, request.Phone, request.DateOfBirth, request.CurrentCity);

                userProfile.UpdateBasicInfo(basicInfo);

                _context.UserProfiles.Update(userProfile);

                await _context.SaveChangesAsync();

                result.Payload = userProfile;

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result.IsError = true;
                result.Errors.Add(new Error { Code = ErrorCode.ServerError, Message = e.Message});
            }


            return result;
        }
    }
}

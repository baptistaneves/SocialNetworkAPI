using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Application.Enums;
using SocialNetwork.Application.Models;
using SocialNetwork.Application.UserProfiles.Commands;
using SocialNetwork.Dal.Context;
using SocialNetwork.Domain.Aggregates.UserProfileAggregate;

namespace SocialNetwork.Application.UserProfiles.CommandHandlers
{
    internal class DeleteUserProfileCommandHandler : IRequestHandler<DeleteUserProfileCommand, OperationResult<UserProfile>>
    {
        private DataContext _context;

        public DeleteUserProfileCommandHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<UserProfile>> Handle(DeleteUserProfileCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<UserProfile>();

            try
            {
                var userProfile = await _context.UserProfiles
                .FirstOrDefaultAsync(up => up.UserProfileId == request.UserProfileId);


                if (userProfile is null)
                {
                    result.IsError = true;
                    result.Errors.Add(new Error
                    {
                        Code = ErrorCode.NotFound,
                        Message = $"No User Profile found with the especified ID {request.UserProfileId}"
                    });
                    return result;
                }

                _context.UserProfiles.Remove(userProfile);

                await _context.SaveChangesAsync();

                result.Payload = userProfile;

            }
            catch (Exception ex)
            {

                result.IsError = true;
                result.Errors.Add(new Error { Code = ErrorCode.UnknownError, Message = ex.Message });
            }
            
            return result;
        }
    }
}

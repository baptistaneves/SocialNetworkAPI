using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Application.Enums;
using SocialNetwork.Application.Identity.Commands;
using SocialNetwork.Application.Models;
using SocialNetwork.Application.UserProfiles;
using SocialNetwork.Dal.Context;

namespace SocialNetwork.Application.Identity.Handlers
{
    public class RemoveAccountCommandHandler : IRequestHandler<RemoveAccountCommand, OperationResult<bool>>
    {
        private readonly DataContext _context;

        public RemoveAccountCommandHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<bool>> Handle(RemoveAccountCommand request, 
            CancellationToken cancellationToken)
        {
            var result = new OperationResult<bool>();

            try
            {
                var identityUser = await _context.Users
                    .FirstOrDefaultAsync(i => i.Id == request.IdentityUserId.ToString(), cancellationToken);

                if(identityUser == null)
                {
                    result.AddError(ErrorCode.IdentityUserDoesNotExist, 
                        IdentityErrorMessages.NonExistentIdentityUser);
                    return result;
                }

                var userProfile = await _context.UserProfiles
                    .FirstOrDefaultAsync(up=> up.IdentityId == request.IdentityUserId.ToString(), cancellationToken); 

                if(userProfile is null)
                {
                    result.AddError(ErrorCode.NotFound, UserProfileErrorMessages.UserProfileNotFound);
                    return result;
                }

                if(identityUser.Id != request.RequestorId.ToString())
                {
                    result.AddError(ErrorCode.UnathorizedAccountRemoval, 
                        IdentityErrorMessages.UnathorizedAccountRemoval);

                    return result;
                }

                _context.UserProfiles.Remove(userProfile);
                _context.Users.Remove(identityUser);
                await _context.SaveChangesAsync(cancellationToken);

                result.Payload = true;

            }
            catch (Exception ex)
            {
                result.AddUnknownError($"{ex.Message}");
            }

            return result;
        }
    }
}

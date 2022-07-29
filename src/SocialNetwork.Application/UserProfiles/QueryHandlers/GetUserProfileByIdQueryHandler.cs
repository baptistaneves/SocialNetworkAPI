using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Application.Enums;
using SocialNetwork.Application.Models;
using SocialNetwork.Application.UserProfiles.Queries;
using SocialNetwork.Dal.Context;
using SocialNetwork.Domain.Aggregates.UserProfileAggregate;

namespace SocialNetwork.Application.UserProfiles.QueryHandlers
{
    internal class GetUserProfileByIdQueryHandler : IRequestHandler<GetUserProfileByIdQuery, OperationResult<UserProfile>>
    {
        private readonly DataContext _context;

        public GetUserProfileByIdQueryHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<UserProfile>> Handle(GetUserProfileByIdQuery request, 
            CancellationToken cancellationToken)
        {
            var result = new OperationResult<UserProfile>();

            try
            {
                var userProfile = await _context.UserProfiles
                .FirstOrDefaultAsync(up => up.UserProfileId == request.UserProfileId, cancellationToken);

                if (userProfile is null)
                {
                    result.AddError(ErrorCode.NotFound,
                       string.Format(UserProfileErrorMessages.UserProfileNotFound, request.UserProfileId));
                    return result;
                }

                result.Payload = userProfile;
            }
            catch (Exception ex)
            {
                result.AddUnknownError($"{ex.Message}");
            }
            
            return result;
        }
    }
}

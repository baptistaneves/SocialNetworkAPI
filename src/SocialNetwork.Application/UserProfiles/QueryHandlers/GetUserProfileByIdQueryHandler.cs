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

            var userProfile =  await _context.UserProfiles
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

            result.Payload = userProfile;

            return result;
        }
    }
}

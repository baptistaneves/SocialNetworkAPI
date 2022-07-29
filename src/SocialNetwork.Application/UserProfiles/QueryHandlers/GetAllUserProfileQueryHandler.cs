using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Application.Enums;
using SocialNetwork.Application.Models;
using SocialNetwork.Application.UserProfiles.Queries;
using SocialNetwork.Dal.Context;
using SocialNetwork.Domain.Aggregates.UserProfileAggregate;

namespace SocialNetwork.Application.UserProfiles.QueryHandlers
{
    internal class GetAllUserProfileQueryHandler : IRequestHandler<GetAllUserProfileQuery, 
        OperationResult<IEnumerable<UserProfile>>>
    {
        private readonly DataContext _context;

        public GetAllUserProfileQueryHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<IEnumerable<UserProfile>>> Handle(GetAllUserProfileQuery request, 
            CancellationToken cancellationToken)
        {
            var result = new OperationResult<IEnumerable<UserProfile>>();

            try
            {
                result.Payload = await _context.UserProfiles.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                result.AddUnknownError($"{ex.Message}");
            }

            return result;
        }
    }
}

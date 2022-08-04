using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Application.Enums;
using SocialNetwork.Application.Identity.Dtos;
using SocialNetwork.Application.Identity.Queries;
using SocialNetwork.Application.Models;
using SocialNetwork.Dal.Context;

namespace SocialNetwork.Application.Identity.QueryHandlers
{
    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, 
        OperationResult<IdentityUserProfileDto>>
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;
        private readonly OperationResult<IdentityUserProfileDto> _result;

        public GetCurrentUserQueryHandler(DataContext context,
                                          UserManager<IdentityUser> userManager, 
                                          IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _result = new OperationResult<IdentityUserProfileDto>();
        }

        public async Task<OperationResult<IdentityUserProfileDto>> Handle(GetCurrentUserQuery request, 
            CancellationToken cancellationToken)
        {
            var identity = await _userManager.FindByIdAsync(request.IdentityId.ToString());

            if (identity == null)
            {
                _result.AddError(ErrorCode.NotFound, IdentityErrorMessages.NonExistentIdentityUser);
                return _result;
            }

            var userProfile = await _context.UserProfiles
                .FirstOrDefaultAsync(up => up.IdentityId == request.IdentityId.ToString(), cancellationToken);

            _result.Payload = _mapper.Map<IdentityUserProfileDto>(userProfile);
            _result.Payload.UserName = identity.UserName;

            return _result;
        }
    }
}

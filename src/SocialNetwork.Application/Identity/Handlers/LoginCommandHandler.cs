using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Application.Enums;
using SocialNetwork.Application.Identity.Commands;
using SocialNetwork.Application.Identity.Dtos;
using SocialNetwork.Application.Models;
using SocialNetwork.Application.Services;
using SocialNetwork.Dal.Context;
using SocialNetwork.Domain.Aggregates.UserProfileAggregate;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SocialNetwork.Application.Identity.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, OperationResult<IdentityUserProfileDto>>
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DataContext _context;
        private readonly IdentityService _identityService;
        private readonly OperationResult<IdentityUserProfileDto> _result;
        private readonly IMapper _mapper;

        public LoginCommandHandler(DataContext context,
                                   UserManager<IdentityUser> userManager,
                                   SignInManager<IdentityUser> signInManager,
                                   IdentityService identityService, 
                                   IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _identityService = identityService;
            _result = new OperationResult<IdentityUserProfileDto>();
            _mapper = mapper;
        }

        public async Task<OperationResult<IdentityUserProfileDto>> Handle(LoginCommand request,
            CancellationToken cancellationToken)
        {

            try
            {
                var identity = await ValidateUserName(_result, request);

                if(_result.IsError) return _result;

                var validPassword = await _signInManager
                    .CheckPasswordSignInAsync(identity, request.Password, true);

                if(validPassword.IsLockedOut)
                {
                    _result.AddError(ErrorCode.LockoutOnFailure, IdentityErrorMessages.LockoutOnFailure);
                    return _result;
                }

                if (validPassword.Succeeded)
                {
                    var userProfile = await _context.UserProfiles
                        .FirstOrDefaultAsync(up => up.IdentityId == identity.Id);

                    _result.Payload = _mapper.Map<IdentityUserProfileDto>(userProfile);
                    _result.Payload.Token = GetJwtString(identity, userProfile);
                    _result.Payload.UserName = identity.UserName;

                    return _result;
                }

                _result.AddError(ErrorCode.IncorrectPassword, IdentityErrorMessages.IncorrectPassword);
            }
            catch (Exception ex)
            {
                _result.AddUnknownError($"{ex.Message}");
            }

            return _result;
        }

        private async Task<IdentityUser> ValidateUserName(OperationResult<IdentityUserProfileDto> result, LoginCommand request)
        {
            var identity = await _userManager.FindByEmailAsync(request.UserName);

            if (identity is null)
                result.AddError(ErrorCode.IncorrectPassword, IdentityErrorMessages.IncorrectPassword);
            
            return identity;
        }

        private string GetJwtString(IdentityUser identity, UserProfile userProfile)
        {
            var claimsIdentity = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, identity.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, identity.Email),
                    new Claim("IdentityId", identity.Id),
                    new Claim("UserProfileId", userProfile.UserProfileId.ToString())
                });

            var token = _identityService.CreateSecurityToken(claimsIdentity);

            return _identityService.WriteToken(token);
        }
    }
}

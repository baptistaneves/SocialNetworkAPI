using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Application.Enums;
using SocialNetwork.Application.Identity.Commands;
using SocialNetwork.Application.Models;
using SocialNetwork.Application.Services;
using SocialNetwork.Dal.Context;
using SocialNetwork.Domain.Aggregates.UserProfileAggregate;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SocialNetwork.Application.Identity.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, OperationResult<string>>
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DataContext _context;
        private readonly IdentityService _identityService;

        public LoginCommandHandler(DataContext context,
                                   UserManager<IdentityUser> userManager,
                                   SignInManager<IdentityUser> signInManager, 
                                   IdentityService identityService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _identityService = identityService;
        }

        public async Task<OperationResult<string>> Handle(LoginCommand request,
            CancellationToken cancellationToken)
        {
            var result = new OperationResult<string>();

            try
            {
                var identity = await ValidateUserName(result, request);

                if(identity == null) return result;

                var validPassword = await _signInManager
                    .CheckPasswordSignInAsync(identity, request.Password, true);

                if(validPassword.IsLockedOut)
                {
                    result.IsError = true;
                    result.Errors.Add(new Error { Code = ErrorCode.LockoutOnFailure,
                        Message = $"User temporarily blocked for invalid attempts."
                    });

                    return result;
                }

                if (validPassword.Succeeded)
                {
                    var userProfile = await _context.UserProfiles
                        .FirstOrDefaultAsync(up => up.IdentityId == identity.Id);

                    result.Payload = GetJwtString(identity, userProfile);

                    return result;
                }

                result.IsError = true;
                result.Errors.Add(new Error { Code = ErrorCode.IncorrectPassword,
                    Message = $"Wrong username or password. Login failed."
                });

            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Errors.Add(new Error { Code = ErrorCode.UnknownError, Message = ex.Message });
            }

            return result;
        }

        private async Task<IdentityUser> ValidateUserName(OperationResult<string> result, LoginCommand request)
        {
            var identity = await _userManager.FindByEmailAsync(request.UserName);

            if (identity is null)
            {
                result.IsError = true;
                result.Errors.Add(new Error
                {
                    Code = ErrorCode.IncorrectPassword,
                    Message = $"Wrong username or password. Login failed."
                });

                return null;
            }

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

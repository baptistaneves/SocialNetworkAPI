using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Application.Enums;
using SocialNetwork.Application.Identity.Commands;
using SocialNetwork.Application.Models;
using SocialNetwork.Application.Services;
using SocialNetwork.Dal.Context;
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
                var identityUser = await _userManager.FindByEmailAsync(request.UserName);

                if (identityUser is null)
                {
                    result.IsError = true;
                    result.Errors.Add(new Error { Code = ErrorCode.IncorrectPassword,
                        Message = $"Wrong username or password. Login failed."
                    });

                    return result;
                }

                var validPassword = await _signInManager
                    .CheckPasswordSignInAsync(identityUser, request.Password, true);

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
                        .FirstOrDefaultAsync(up => up.IdentityId == identityUser.Id);

                    var claimsIdentity = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, identityUser.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, identityUser.Email),
                        new Claim("IdentityId", identityUser.Id),
                        new Claim("UserProfileId", userProfile.UserProfileId.ToString())
                    });

                    var token = _identityService.CreateSecurityToken(claimsIdentity);

                    result.Payload = _identityService.WriteToken(token);

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
    }
}

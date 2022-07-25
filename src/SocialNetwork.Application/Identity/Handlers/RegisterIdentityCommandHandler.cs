using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SocialNetwork.Application.Enums;
using SocialNetwork.Application.Identity.Commands;
using SocialNetwork.Application.Models;
using SocialNetwork.Application.Options;
using SocialNetwork.Dal.Context;
using SocialNetwork.Domain.Aggregates.UserProfileAggregate;
using SocialNetwork.Domain.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialNetwork.Application.Identity.Handlers
{
    public class RegisterIdentityCommandHandler : IRequestHandler<RegisterIdentityCommand, OperationResult<string>>
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;

        public RegisterIdentityCommandHandler(UserManager<IdentityUser> userManager, DataContext context,
                                              IOptions<JwtSettings> jwtsettings)
        {
            _userManager = userManager;
            _context = context;
            _jwtSettings = jwtsettings.Value;
        }

        public async Task<OperationResult<string>> Handle(RegisterIdentityCommand request, 
            CancellationToken cancellationToken)
        {
            var result = new OperationResult<string>();

            try
            {
                var existingIdentity = await _userManager.FindByEmailAsync(request.UserName);

                if(existingIdentity != null)
                {
                    result.IsError = true;
                    var error = new Error { Code = ErrorCode.IdentityUserAlreadyExists, 
                        Message = $"Provided email address already exists. Cannot register new user." };
                    result.Errors.Add(error);

                    return result;
                }

                var identity = new IdentityUser
                {
                    UserName = request.UserName,
                    Email = request.UserName
                };

                using var transaction = _context.Database.BeginTransaction();
                 
                var createdIdentity = await _userManager.CreateAsync(identity, request.Password);

                if(!createdIdentity.Succeeded)
                {
                    await transaction.RollbackAsync();

                    result.IsError = true;

                    foreach(var identintyError in createdIdentity.Errors)
                    {
                        result.Errors.Add(new Error
                        {
                            Code = ErrorCode.IdentityCreationFaild,
                            Message = identintyError.Description
                        });
                    }

                    return result;
                }

                var profileInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, 
                    request.UserName, request.Phone, request.DateOfBirth, request.CurrentCity);

                var userProfile = UserProfile.CreateUserProfile(identity.Id, profileInfo);

                try
                {
                    _context.UserProfiles.Add(userProfile);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {  
                    await transaction.RollbackAsync();
                    throw;
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSettings.SigningKey);
                //Here is to provide to the token the exact configurations or options what must be used to create the token
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, identity.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, identity.Email),
                        new Claim("IdentityId", identity.Id),
                        new Claim("UserProfileId", userProfile.UserProfileId.ToString())
                    }),
                    Expires = DateTime.Now.AddHours(2),
                    Audience = _jwtSettings.Audiences[0],
                    Issuer = _jwtSettings.Issuer,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                result.Payload = tokenHandler.WriteToken(token);
            }

            catch (UserProfileNotValidException ex)
            {
                result.IsError = true;
                ex.ValidationErrors.ForEach(error =>
                {
                    result.Errors.Add(new Error { Code = ErrorCode.ValidationError, Message = $"{error}" });
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

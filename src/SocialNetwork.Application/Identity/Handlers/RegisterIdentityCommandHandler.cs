using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using SocialNetwork.Application.Enums;
using SocialNetwork.Application.Identity.Commands;
using SocialNetwork.Application.Models;
using SocialNetwork.Application.Services;
using SocialNetwork.Dal.Context;
using SocialNetwork.Domain.Aggregates.UserProfileAggregate;
using SocialNetwork.Domain.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SocialNetwork.Application.Identity.Handlers
{
    public class RegisterIdentityCommandHandler : IRequestHandler<RegisterIdentityCommand, OperationResult<string>>
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IdentityService _identityService;

        public RegisterIdentityCommandHandler(UserManager<IdentityUser> userManager, 
                                              DataContext context,
                                              IdentityService identityService)
        {
            _userManager = userManager;
            _context = context;
            _identityService = identityService;
        }

        public async Task<OperationResult<string>> Handle(RegisterIdentityCommand request, 
            CancellationToken cancellationToken)
        {
            var result = new OperationResult<string>();

            try
            {
                var creationValidated = await ValidateIdentityDoesNotExist(result, request);
                if (!creationValidated) return result;

                await using var transaction = _context.Database.BeginTransaction();
                
                var identity = await CreateIdentityUserAsync(result, request, transaction);
                if (identity is null) return result;

                var userProfile = await CreateUserProfile(result, request, transaction, identity);
                await transaction.CommitAsync();

                result.Payload = GetJwtString(identity, userProfile);
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

        private async Task<bool> ValidateIdentityDoesNotExist(OperationResult<string> result, 
            RegisterIdentityCommand request)
        {
            var existingIdentity = await _userManager.FindByEmailAsync(request.UserName);

            if (existingIdentity != null)
            {
                result.IsError = true;
                var error = new Error
                {
                    Code = ErrorCode.IdentityUserAlreadyExists,
                    Message = $"Provided email address already exists. Cannot register new user."
                };
                result.Errors.Add(error);

                return false;
            }

            return true;
        }

        private async Task<IdentityUser> CreateIdentityUserAsync(OperationResult<string> result, 
            RegisterIdentityCommand request, IDbContextTransaction transaction)
        {
            var identity = new IdentityUser { Email = request.UserName, UserName = request.UserName };

            var createdIdentity = await _userManager.CreateAsync(identity, request.Password);

            if (!createdIdentity.Succeeded)
            {
                await transaction.RollbackAsync();

                result.IsError = true;

                foreach (var identintyError in createdIdentity.Errors)
                {
                    result.Errors.Add(new Error
                    {
                        Code = ErrorCode.IdentityCreationFaild,
                        Message = identintyError.Description
                    });
                }

                return null;
            }

            return identity;
        }

        private async Task<UserProfile> CreateUserProfile(OperationResult<string> result,
            RegisterIdentityCommand request, IDbContextTransaction transaction, IdentityUser identity)
        {
            var profileInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName,
                    request.UserName, request.Phone, request.DateOfBirth, request.CurrentCity);

            var userProfile = UserProfile.CreateUserProfile(identity.Id, profileInfo);

            try
            {
                _context.UserProfiles.Add(userProfile);
                await _context.SaveChangesAsync();
                return userProfile;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
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

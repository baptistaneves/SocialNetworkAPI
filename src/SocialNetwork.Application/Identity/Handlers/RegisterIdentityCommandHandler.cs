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
                await ValidateIdentityDoesNotExist(result, request);
                if (result.IsError) return result;

                await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
                
                var identity = await CreateIdentityUserAsync(result, request, transaction, cancellationToken);
                if (result.IsError) return result;

                var userProfile = await CreateUserProfile(result, request, transaction, identity, cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                result.Payload = GetJwtString(identity, userProfile);
            }

            catch (UserProfileNotValidException ex)
            {
                ex.ValidationErrors.ForEach(error => result.AddError(ErrorCode.ValidationError, error));
            }

            catch (Exception ex)
            {
                result.AddUnknownError($"{ex.Message}");
            }

            return result;
        }

        private async Task ValidateIdentityDoesNotExist(OperationResult<string> result, 
            RegisterIdentityCommand request)
        {
            var existingIdentity = await _userManager.FindByEmailAsync(request.UserName);

            if (existingIdentity != null)
                result.AddError(ErrorCode.IdentityUserAlreadyExists, IdentityErrorMessages.IdentityUserAlreadyExists);  
                
        }

        private async Task<IdentityUser> CreateIdentityUserAsync(OperationResult<string> result, 
            RegisterIdentityCommand request, IDbContextTransaction transaction, CancellationToken cancellationToken)
        {
            var identity = new IdentityUser { Email = request.UserName, UserName = request.UserName };

            var createdIdentity = await _userManager.CreateAsync(identity, request.Password);

            if (!createdIdentity.Succeeded)
            {
                await transaction.RollbackAsync(cancellationToken);

                foreach (var identintyError in createdIdentity.Errors)
                {
                    result.AddError(ErrorCode.IdentityCreationFaild, identintyError.Description);
                }

            }

            return identity;
        }

        private async Task<UserProfile> CreateUserProfile(OperationResult<string> result,
            RegisterIdentityCommand request, IDbContextTransaction transaction, 
            IdentityUser identity, CancellationToken cancellationToken)
        {
           
            try
            {
                var profileInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName,
                   request.UserName, request.Phone, request.DateOfBirth, request.CurrentCity);

                var userProfile = UserProfile.CreateUserProfile(identity.Id, profileInfo);

                _context.UserProfiles.Add(userProfile);
                await _context.SaveChangesAsync(cancellationToken);
                return userProfile;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
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

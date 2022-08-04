using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using SocialNetwork.Application.Enums;
using SocialNetwork.Application.Identity.Commands;
using SocialNetwork.Application.Identity.Dtos;
using SocialNetwork.Application.Models;
using SocialNetwork.Application.Services;
using SocialNetwork.Dal.Context;
using SocialNetwork.Domain.Aggregates.UserProfileAggregate;
using SocialNetwork.Domain.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SocialNetwork.Application.Identity.Handlers
{
    public class RegisterIdentityCommandHandler : IRequestHandler<RegisterIdentityCommand, OperationResult<IdentityUserProfileDto>>
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IdentityService _identityService;
        private readonly OperationResult<IdentityUserProfileDto> _result;
        private readonly IMapper _mapper;

        public RegisterIdentityCommandHandler(UserManager<IdentityUser> userManager,
                                              DataContext context,
                                              IdentityService identityService, 
                                              IMapper mapper)
        {
            _userManager = userManager;
            _context = context;
            _identityService = identityService;
            _result = new OperationResult<IdentityUserProfileDto>();
            _mapper = mapper;
        }

        public async Task<OperationResult<IdentityUserProfileDto>> Handle(RegisterIdentityCommand request, 
            CancellationToken cancellationToken)
        {
            try
            {
                await ValidateIdentityDoesNotExist(_result, request);
                if (_result.IsError) return _result;

                await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
                
                var identity = await CreateIdentityUserAsync(_result, request, transaction, cancellationToken);
                if (_result.IsError) return _result;

                var userProfile = await CreateUserProfile(_result, request, transaction, identity, cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                _result.Payload = _mapper.Map<IdentityUserProfileDto>(userProfile);
                _result.Payload.Token = GetJwtString(identity, userProfile);
                _result.Payload.UserName = identity.UserName;
            }

            catch (UserProfileNotValidException ex)
            {
                ex.ValidationErrors.ForEach(error => _result.AddError(ErrorCode.ValidationError, error));
            }

            catch (Exception ex)
            {
                _result.AddUnknownError($"{ex.Message}");
            }

            return _result;
        }

        private async Task ValidateIdentityDoesNotExist(OperationResult<IdentityUserProfileDto> result, 
            RegisterIdentityCommand request)
        {
            var existingIdentity = await _userManager.FindByEmailAsync(request.UserName);

            if (existingIdentity != null)
                result.AddError(ErrorCode.IdentityUserAlreadyExists, IdentityErrorMessages.IdentityUserAlreadyExists);  
                
        }

        private async Task<IdentityUser> CreateIdentityUserAsync(OperationResult<IdentityUserProfileDto> result, 
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

        private async Task<UserProfile> CreateUserProfile(OperationResult<IdentityUserProfileDto> result,
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

using SocialNetwork.Api.Contracts.Identity;
using SocialNetwork.Application.Identity.Commands;
using SocialNetwork.Application.Identity.Dtos;

namespace SocialNetwork.Api.MappingProfile
{
    public class IdentityMapping : Profile
    {
        public IdentityMapping()
        {
            CreateMap<UserRegistration, RegisterIdentityCommand>();
            CreateMap<Login, LoginCommand>();
            CreateMap<IdentityUserProfileDto, IdentityUserProfile>();
        }
    }
}

using SocialNetwork.Api.Contracts.Identity;
using SocialNetwork.Application.Identity.Commands;

namespace SocialNetwork.Api.MappingProfile
{
    public class IdentityMapping : Profile
    {
        public IdentityMapping()
        {
            CreateMap<UserRegistration, RegisterIdentityCommand>();
        }
    }
}

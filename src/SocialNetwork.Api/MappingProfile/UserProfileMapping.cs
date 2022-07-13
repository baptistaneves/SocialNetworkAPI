using AutoMapper;
using SocialNetwork.Api.Contracts.UserProfiles.Requests;
using SocialNetwork.Api.Contracts.UserProfiles.Responses;
using SocialNetwork.Application.UserProfiles.Commands;
using SocialNetwork.Domain.Aggregates.UserProfileAggregate;

namespace SocialNetwork.Api.MappingProfile
{
    public class UserProfileMapping : Profile
    {
        public UserProfileMapping()
        {
            CreateMap<UserProfileCreateUpdate, CreateUserCommand>();
            CreateMap<UserProfileCreateUpdate, UpdateUserProfileBasicInfoCommand>();
            CreateMap<UserProfile, UserProfileResponse>();
            CreateMap<BasicInfo, BasicInformation>();
        }
    }
}

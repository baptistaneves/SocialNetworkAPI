using AutoMapper;
using SocialNetwork.Api.Contracts.Posts.Responses;
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
            CreateMap<UserProfileCreate, UpdateUserProfileBasicInfoCommand>();
            CreateMap<UserProfile, UserProfileResponse>();
            CreateMap<BasicInfo, BasicInformation>();

            CreateMap<UserProfile, InteractionUser>()
                .ForMember(dest
                    => dest.FullName, opt
                    => opt.MapFrom(src
                    => src.BasicInfo.FirstName + " " + src.BasicInfo.LastName))
                .ForMember(dest
                    => dest.City, opt
                    => opt.MapFrom(src
                    => src.BasicInfo.CurrentCity));
        }
    }
}

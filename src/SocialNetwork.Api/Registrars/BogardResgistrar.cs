using MediatR;
using SocialNetwork.Application.UserProfiles.Queries;

namespace SocialNetwork.Api.Registrars
{
    public class BogardResgistrar : IWebApplicationBuilderRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddMediatR(typeof(GetAllUserProfileQuery));
        }
    }
}

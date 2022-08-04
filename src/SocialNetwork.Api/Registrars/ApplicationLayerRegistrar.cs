using SocialNetwork.Application.Identity.MappingProfiles;
using SocialNetwork.Application.Services;

namespace SocialNetwork.Api.Registrars
{
    public class ApplicationLayerRegistrar : IWebApplicationBuilderRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IdentityService>();
            builder.Services.AddAutoMapper(typeof(IdentityProfiles));
        }
    }
}
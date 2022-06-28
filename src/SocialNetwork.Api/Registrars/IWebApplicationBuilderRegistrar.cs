namespace SocialNetwork.Api.Registrars
{
    public interface IWebApplicationBuilderRegistrar : IRegistrar
    {
        void RegisterServices(WebApplicationBuilder builder);
    }
}

namespace SocialNetwork.Api.Registrars
{
    public interface IWebApplicationRegistrar : IRegistrar
    {
        void RegisterPipeLineComponents(WebApplication app);
    }
}

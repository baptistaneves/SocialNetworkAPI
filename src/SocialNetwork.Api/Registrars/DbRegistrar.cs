namespace SocialNetwork.Api.Registrars
{
    public class DbRegistrar : IWebApplicationBuilderRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            var cs = builder.Configuration.GetConnectionString("Default");

            builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(cs));

            //This line of code add a bundle identity services in our dependecy injection
            builder.Services.AddIdentityCore<IdentityUser>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddSignInManager<SignInManager<IdentityUser>>()
            .AddEntityFrameworkStores<DataContext>();
        }
    }
}

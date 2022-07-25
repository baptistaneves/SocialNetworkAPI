namespace SocialNetwork.Api.Registrars
{
    public class IdentityRegistrar : IWebApplicationBuilderRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            var jwtSetting = new JwtSettings();

            //By the time this line of code get executed this object (jwtSetting) should contain all
            //the properties that we have in the appsettings.json file acording to the corresponding section
            builder.Configuration.Bind(nameof(JwtSettings), jwtSetting);

            //This line of code enables mapping configuration settings from the appsettings.json
            //to our JwtSettings class
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));
           
            builder.Services
                .AddAuthentication(a=>
                {
                    a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    a.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(jwt =>
                {
                    jwt.SaveToken = true;
                    //Here we perform how we want to validate the token when the request comes
                    jwt.TokenValidationParameters = new TokenValidationParameters
                    {
                        //The key which is used to compare with incoming token
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSetting.SigningKey)),
                        //To validate the Issuer token, if the issuer token is not us, then we do not valid the token
                        ValidateIssuer = true,
                        ValidIssuer = jwtSetting.Issuer,
                        //To validate our audience
                        ValidateAudience = true,
                        ValidAudiences = jwtSetting.Audiences,
                        //If we want the expiration time of token present in our token
                        RequireExpirationTime = false,
                        ValidateLifetime = true,
                    };
                    //Our default audince
                    jwt.Audience = jwtSetting.Audiences[0]; 
                    jwt.ClaimsIssuer = jwtSetting.Issuer;
                });
        }
    }
}

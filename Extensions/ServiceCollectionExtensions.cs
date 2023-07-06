using Blog.Api.Managers;
using Blog.Api.Options;
using Blog.Api.Providers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Api.Extensions;

public static class ServiceCollectionExtensions
{
    private static void AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(nameof(JwtOptions));
        services.Configure<JwtOptions>(section);
    
        var jwtOptions = section.Get<JwtOptions>()!;
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {

            var signingKey = System.Text.Encoding.UTF32.GetBytes(jwtOptions.SigningKey);
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidAudience = jwtOptions.ValidAudience,
                ValidIssuer = jwtOptions.ValidIssuer,
                ValidateAudience = true,
                ValidateIssuer = true,
                IssuerSigningKey = new SymmetricSecurityKey(signingKey),
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            options.Events = new JwtBearerEvents()
            {
                OnMessageReceived = async context =>
                {
                    if (string.IsNullOrEmpty(context.Token))
                    {
                        var accessToken = context.Request.Query["token"];
                        context.Token = accessToken;
                    }
                }
            };
        }); 
    }
    public static void AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddJwt(configuration);

        services.AddScoped<TokenManager>();
        services.AddScoped<UserManager>();
        services.AddHttpContextAccessor();
        services.AddScoped<UserProvider>();
    }
}
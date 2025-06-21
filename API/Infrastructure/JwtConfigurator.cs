using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Infrastructure;

public static class JwtConfigurator
{
    public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(opts =>
        {
            opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            
        })
        .AddJwtBearer(opts =>
        {
            var jwtConfig = configuration.GetSection("Jwt");
            opts.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfig["Issuer"],
                ValidAudience = jwtConfig["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]!))
            };
            Console.WriteLine(JsonSerializer.Serialize(opts.TokenValidationParameters));
            Console.WriteLine(JsonSerializer.Serialize(opts.TokenValidationParameters));
        });
        services.AddAuthorization();

    }

    public static void ConfigureJwt(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
    
}
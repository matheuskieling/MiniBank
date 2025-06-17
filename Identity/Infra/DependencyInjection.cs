using Core.Database;
using Identity.Repositories;
using Identity.Repositories.Interfaces;
using Identity.Services;
using Identity.Services.Interfaces;

namespace Identity.Infra;

public static class DependencyInjection
{
    public static void InitDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<DbSession>(_ =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString),
                    "Connection String not found. Configure it in appsettings.json");
            }
            return new DbSession(connectionString);
        });
    }
}
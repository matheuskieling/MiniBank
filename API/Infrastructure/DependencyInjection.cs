using API.Repository;
using API.Services;
using Core.Database;
using Core.Domain;

namespace API.Infrastructure;

public static class DependencyInjection
{
    public static void InitDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<DbSession>(_ =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString), "Connection string 'DefaultConnection' is not configured.");
            return new DbSession(connectionString);
        });
        
        services.AddScoped<WalletService>();
        services.AddScoped<WalletRepository>();
    }
}
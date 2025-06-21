using API.Repository;
using API.Repository.Interfaces;
using API.Services;
using API.Services.Interfaces;
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
        
        services.AddScoped<IWalletService, WalletService>();
        services.AddScoped<IWalletRepository, WalletRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<ITransactionService, TransactionService>();
    }
}
using FluentMigrator.Runner;

namespace API.Infrastructure;

public static class ConfigureFluentMigrator
{
    public static void AddFluentMigrator(this IServiceCollection services, IConfiguration configuration)
    {
        // Register FluentMigrator services
        services.AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddPostgres()
                .WithGlobalConnectionString(configuration.GetConnectionString("DefaultConnection"))
                .ScanIn(typeof(ConfigureFluentMigrator).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole());
    }
    
    public static void UseMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }
    
}
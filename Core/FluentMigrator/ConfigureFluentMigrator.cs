using System.Reflection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.VersionTableInfo;

namespace Core.FluentMigrator;

public static class ConfigureFluentMigrator
{
    public static void AddFluentMigrator(this IServiceCollection services, IConfiguration configuration, Assembly migrationsAssembly, string schema)
    {
        // Register FluentMigrator services
        services.AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddPostgres()
                .WithVersionTable(new CustomVersionTable(schema))
                .WithGlobalConnectionString(configuration.GetConnectionString("DefaultConnection"))
                .ScanIn(migrationsAssembly).For.Migrations()
            )
            .AddLogging(lb => lb.AddFluentMigratorConsole());
    }
    
    public static void UseMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

        // Get the list of pending migrations
        var pendingMigrations = runner.MigrationLoader.LoadMigrations()
            .Select(m => m.Value.Migration.GetType().Name);

        // Log or inspect the pending migrations
        foreach (var migration in pendingMigrations)
        {
            Console.WriteLine($"Pending migration: {migration}");
        }

        runner.MigrateUp();
    }
    
}

[Obsolete("Obsolete")]
public class CustomVersionTable(string schema) : DefaultVersionTableMetaData
{
    public override string SchemaName => schema;
}

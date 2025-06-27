using System.Reflection;
using Core.Database;
using Core.FluentMigrator;
using FluentMigrator.Runner;
using Identity.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Testcontainers.PostgreSql;

namespace IntegrationTests.Infra;

public class IdentityWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly PostgreSqlContainer _container = TestContainerFactory.GetContainer();
    public IdentityWebApplicationFactory()
    {
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var connectionString = $"{_container.GetConnectionString()};Include Error Detail=true;Search Path=identity;";
        builder.UseEnvironment("IntegrationTest");
        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            configBuilder.AddJsonFile("appsettings.json", optional: false);
            configBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "ConnectionStrings:DefaultConnection", connectionString }
            }!);
        });

        builder.ConfigureServices((context, services) =>
        {
            services.RemoveAll<DbSession>();
            services.AddScoped<DbSession>(_ => new DbSession(connectionString));
            services.AddFluentMigrator(context.Configuration, "identity");
        });

        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        });
    }
}
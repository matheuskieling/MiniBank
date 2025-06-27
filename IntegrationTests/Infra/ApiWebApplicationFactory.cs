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

public class ApiWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class 
{
    private readonly PostgreSqlContainer _container;
    private string _searchPath;

    public ApiWebApplicationFactory(PostgreSqlContainer container, string searchPath)
    {
        _searchPath = searchPath;
        _container = container;
        Task.Run(() => _container.StartAsync()).Wait();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        if (_container == null)
        {
            throw new InvalidOperationException("PostgreSQL container is not initialized.");
        }
        if (string.IsNullOrEmpty(_searchPath))
        {
            throw new ArgumentNullException(nameof(_searchPath), "The searchPath (schema) should be informed.");
        }
        
        var connectionString = $"{_container.GetConnectionString()};Include Error Detail=true;Search Path={_searchPath};";
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
            services.RemoveAll<IMigrationRunner>();
            services.AddFluentMigrator(context.Configuration, "bank");
            services.AddScoped<ITokenService, TokenService>();
        });

        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        });
    }
}
using DotNet.Testcontainers.Containers;
using Testcontainers.PostgreSql;

namespace IntegrationTests.Infra;

public static class TestContainerFactory
{
    private static Lazy<PostgreSqlContainer> _container = new Lazy<PostgreSqlContainer>(CreateContainer);
    private static bool _isDisposed = false;

    public static PostgreSqlContainer CreateContainer()
    {
        return new PostgreSqlBuilder()
            .WithDatabase("testdb")
            .WithUsername("testuser")
            .WithPassword("testpassword")
            .WithCleanUp(true)
            .WithAutoRemove(true)
            .Build();
    }

    public static PostgreSqlContainer GetContainer()
    {
        if (_isDisposed)
        {
            _container = new Lazy<PostgreSqlContainer>(CreateContainer);
            _isDisposed = false;
        }
        if (_container.Value.State != TestcontainersStates.Running)
        {
            Task.Run(() => _container.Value.StartAsync()).Wait();
        }
        return _container.Value;
    }

    public static void DisposeContainer()
    {
        if (_container.Value.State == TestcontainersStates.Running)
        {
            Task.Run(() => _container.Value.StopAsync()).Wait();
            _container.Value.DisposeAsync().AsTask().Wait();
            _isDisposed = true;
        }
    }
}
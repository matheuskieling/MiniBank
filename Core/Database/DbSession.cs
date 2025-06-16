using System.Data;
using Npgsql;

namespace Core.Database;

public class DbSession : IDisposable
{
    public IDbConnection Connection { get; set; }
    public IDbTransaction? Transaction { get; set; }
    public readonly string Schema;
    public readonly string ConnectionString;
    public readonly NpgsqlDataSource? _dataSource;
    
    public DbSession(string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        var builder = new NpgsqlConnectionStringBuilder(connectionString);
        
        if (string.IsNullOrEmpty(builder.SearchPath))
        {
            throw new ArgumentNullException("The searchPath (schema) should be informed in the connection string.");
        }

        Schema = builder.SearchPath;
        ConnectionString = builder.ConnectionString;
        _dataSource = CreateDataSource(builder.ConnectionString);
        Connection = _dataSource.OpenConnection();
    }

    private NpgsqlDataSource CreateDataSource(string connectionString)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        return dataSourceBuilder.Build();
    }
    
    public void Dispose()
    {
        Connection?.Dispose();
    }
}
using Core.Data;
using Core.Database;
using Dapper;
using Npgsql;

namespace Core.Domain;

public abstract class BaseRepository<T> : IBaseRepository<T>
    where T : class
{
    protected readonly DbSession Session;
    protected readonly ILogger<T> Logger;
    public BaseRepository(DbSession session, ILogger<T> logger)
    {
        Session = session ?? throw new ArgumentNullException(nameof(session));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public async Task<TReturn?> QueryFirstOrDefaultAsync<TReturn>(string query, object? param = null)
    {
        try
        {
            return await Session.Connection.QueryFirstOrDefaultAsync<TReturn>(query, param, Session.Transaction);
        }
        catch (Exception ex)
        {
            Logger.LogError($"Error on executing {query} with params {param}, Ex: {ex.Message}");
            throw;
        }
    }
    
    public async Task<CommandResult<TReturn>> ExecuteCommand<TReturn>(IBaseCommand command)
    {
        try
        {
            if (!command.IsValid())
            {
                return new CommandResult<TReturn>(command.ValidationResults);
            }
            var result = await Session.Connection.QueryFirstOrDefaultAsync<TReturn>(command.Script, command.Param, Session.Transaction);
            
            return new CommandResult<TReturn>(result);
        }
        catch (PostgresException ex)
        {
            Logger.LogError($"Error on executing {command.Script} with params {command.Param}, Ex: {ex.Message}");
            return new CommandResult<TReturn>(ex.Message, ex.SqlState);
        }
    }
    
    public async Task<IEnumerable<TReturn>> QueryAsync<TReturn>(string query, object? param = null)
    {
        try
        {
            return await Session.Connection.QueryAsync<TReturn>(query, param, Session.Transaction);
        }
        catch (Exception ex)
        {
            Logger.LogError($"Error on executing {query} with params {param}, Ex: {ex.Message}");
            throw;
        }
    }
}
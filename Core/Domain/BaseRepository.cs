using Core.Data;
using Core.Database;
using Dapper;
using Npgsql;

namespace Core.Domain;

public abstract class BaseRepository<T>(DbSession session, ILogger<T> logger) : IBaseRepository<T>
    where T : class
{
    public async Task<TReturn?> QueryFirstOrDefaultAsync<TReturn>(string query, object? param = null)
    {
        try
        {
            return await session.Connection.QueryFirstOrDefaultAsync<TReturn>(query, param, session.Transaction);
        }
        catch (Exception ex)
        {
            logger.LogError($"Error on executing {query} with params {param}, Ex: {ex.Message}");
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
            var result = await session.Connection.QueryFirstOrDefaultAsync<TReturn>(command.Script, command.Param, session.Transaction);
            
            return new CommandResult<TReturn>(result);
        }
        catch (PostgresException ex)
        {
            logger.LogError($"Error on executing {command.Script} with params {command.Param}, Ex: {ex.Message}");
            return new CommandResult<TReturn>(ex.Message, ex.SqlState);
        }
    }
    
    public async Task<IEnumerable<TReturn>> QueryAsync<TReturn>(string query, object? param = null)
    {
        try
        {
            return await session.Connection.QueryAsync<TReturn>(query, param, session.Transaction);
        }
        catch (Exception ex)
        {
            logger.LogError($"Error on executing {query} with params {param}, Ex: {ex.Message}");
            throw;
        }
    }
}
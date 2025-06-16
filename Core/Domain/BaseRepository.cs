using Core.Database;
using Dapper;

namespace Core.Domain;

public abstract class BaseRepository<T>(DbSession session) : IBaseRepository<T>
    where T : class
{
    private readonly DbSession _session = session;

    public async Task<TReturn?> QueryFirstOrDefaultAsync<TReturn>(string query, object? param = null)
    {
        return await _session.Connection.QueryFirstOrDefaultAsync<TReturn>(query, param);
    }
}
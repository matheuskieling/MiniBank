using Core.Database;

namespace Core.Data;

public interface IUnitOfWork : IDisposable
{
    void BeginTransaction();
    void Commit();
    void Rollback();
    bool HasActiveTransaction();
}
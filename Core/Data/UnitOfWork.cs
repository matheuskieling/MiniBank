using Core.Database;

namespace Core.Data;

public class UnitOfWork : IUnitOfWork
{
    private DbSession Session { get; }

    public UnitOfWork(DbSession session)
    {
        Session = session;
    }
    
    public void BeginTransaction()
    {
        if (HasActiveTransaction())
        {
            throw new InvalidOperationException("Transaction already started.");
        }
        Session.Transaction = Session.Connection.BeginTransaction();
    }

    public void Commit()
    {
        if (!HasActiveTransaction())
        {
            throw new InvalidOperationException("No transaction started.");
        }
        try 
        {
            Session.Transaction!.Commit();
            Dispose();
            Session.Transaction = null;
        }
        catch (Exception)
        {
            Rollback();
            throw;
        }
    }

    public void Rollback()
    {
        if (!HasActiveTransaction())
        {
            throw new InvalidOperationException("No transaction started.");
        }
        Session.Transaction!.Rollback();
    }

    public bool HasActiveTransaction()
    {
        return Session.Transaction is not null;
    }

    public void Dispose()
    {
        if (HasActiveTransaction())
        {
            Session.Transaction!.Dispose();
        }
    }
}
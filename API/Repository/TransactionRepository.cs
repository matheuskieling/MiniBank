using API.Data.Command;
using API.Models;
using API.Repository.Interfaces;
using Core.Data;
using Core.Database;
using Core.Domain;

namespace API.Repository;

public class TransactionRepository(DbSession dbSession, ILogger<TransactionRepository> logger)
    : BaseRepository<TransactionRepository>(dbSession, logger), ITransactionRepository
{
    public DbSession dbSession { get; set; } = dbSession;

    public async Task<CommandResult<Transaction>> CreateTransaction(Transaction transaction)
    {
        var command = new CreateTransactionCommand(transaction);
        return await ExecuteCommand<Transaction>(command);
    }
}
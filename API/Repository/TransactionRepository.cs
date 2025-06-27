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

    public async Task<CommandResult<Transaction>> CreateTransaction(Transaction transaction)
    {
        var command = new CreateTransactionCommand(transaction);
        return await ExecuteCommand<Transaction>(command);
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsBySenderId(Guid senderId)
    {
        // var query = "SELECT * FROM Transactions WHERE sender_id = @SenderId";
        var query = @"SELECT
                        id as Id,
                        sender_id as SenderId,
                        receiver_id as ReceiverId,
                        amount as Amount,
                        created_at as CreatedAt
                    FROM transactions
                    WHERE sender_id = @SenderId
        ";
        var param = new { SenderId = senderId };
        var result =  await QueryAsync<Transaction>(query, param);
        return result;
    }


        public DbSession GetDbSession()
        {
        return Session;
    }
}
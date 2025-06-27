using API.Models;
using Core.Data;
using Core.Database;

namespace API.Repository.Interfaces;

public interface ITransactionRepository
{
    Task<CommandResult<Transaction>> CreateTransaction(Transaction transaction);
    DbSession GetDbSession();
    Task<IEnumerable<Transaction>> GetTransactionsBySenderId(Guid senderId);
}
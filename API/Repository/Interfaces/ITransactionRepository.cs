using API.Models;
using Core.Data;
using Core.Database;

namespace API.Repository.Interfaces;

public interface ITransactionRepository
{
    DbSession dbSession { get; set; }
    Task<CommandResult<Transaction>> CreateTransaction(Transaction transaction);
}
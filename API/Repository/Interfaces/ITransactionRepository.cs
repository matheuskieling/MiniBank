using API.Models;
using Core.Data;

namespace API.Repository.Interfaces;

public interface ITransactionRepository
{
    Task<CommandResult<Transaction>> CreateTransaction(Transaction transaction);
}
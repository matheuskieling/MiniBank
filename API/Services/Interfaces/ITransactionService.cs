using API.Models;
using Core.Data;

namespace API.Services.Interfaces;

public interface ITransactionService
{
    Task<CommandResult<Transaction>> CreateTransaction(Guid receiverId, long amount);
    Task<IEnumerable<Transaction>> GetTransactionsBySenderId(Guid senderId);
}
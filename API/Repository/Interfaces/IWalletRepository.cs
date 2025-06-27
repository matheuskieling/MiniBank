using API.Models;
using Core.Data;

namespace API.Repository.Interfaces;

public interface IWalletRepository
{
    Task<Wallet?> GetWalletById(Guid id);
    Task<Wallet?> GetWalletByUserId(Guid id);
    Task<IEnumerable<Wallet>> GetWallets();
    Task<CommandResult<Wallet>> CreateWallet(Guid userId);
   Task<bool> AddFundsToWallet(long amount, Guid walletId);
   Task<bool> RemoveFundsFromWallet(long amount, Guid walletId);
}
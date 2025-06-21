using API.Models;
using Core.Data;

namespace API.Services.Interfaces;

public interface IWalletService
{
   Task<CommandResult<Wallet>> CreateWallet(Guid userId);
   Task<Wallet?> GetWalletById(Guid id);
   Task<Wallet?> GetWalletByUserId(Guid userId);
   Task<Wallet?> GetCurrentUserWallet();
   Task<bool> AddFundsToWallet(long amount);
   Task<bool> AddFundsToWallet(Guid walletId, long amount);
   Task<bool> RemoveFundsFromWallet(long amount);
}
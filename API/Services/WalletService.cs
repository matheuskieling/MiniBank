using API.Models;
using API.Repository.Interfaces;
using API.Services.Interfaces;
using Core.Data;

namespace API.Services;

public class WalletService(IWalletRepository walletRepository, ICurrentUserService currentUserService) : IWalletService
{
    public async Task<CommandResult<Wallet>> CreateWallet(Guid userId)
    {
        return await walletRepository.CreateWallet(userId);
    }

    public async Task<IEnumerable<Wallet>> GetWallets()
    {
        return await walletRepository.GetWallets();
    }

    public async Task<Wallet?> GetWalletById(Guid id)
    {
        return await walletRepository.GetWalletById(id);
    }

    public async Task<Wallet?> GetWalletByUserId(Guid userId)
    {
        return await walletRepository.GetWalletByUserId(userId);
    }

    public async Task<Wallet?> GetCurrentUserWallet()
    {
        
        return await walletRepository.GetWalletByUserId(currentUserService.CurrentUser!.UserId);
    }

    public async Task<bool> AddFundsToWallet(long amount)
    {
        var wallet = await GetCurrentUserWallet();
        if (wallet is null)
        {
            throw new InvalidOperationException("Current user wallet not found");
        }
        return await walletRepository.AddFundsToWallet(amount, wallet.Id);
    }
    
    public async Task<bool> RemoveFundsFromWallet(long amount)
    {
        var wallet = await GetCurrentUserWallet();
        if (wallet is null)
        {
            throw new InvalidOperationException("Current user wallet not found");
        }
        return await walletRepository.RemoveFundsFromWallet(amount, wallet.Id);
    }
    
    public async Task<bool> AddFundsToWallet(Guid walletId, long amount)
    {
        var wallet = await GetWalletById(walletId);
        if (wallet is null)
        {
            throw new InvalidOperationException("Current user wallet not found");
        }
        return await walletRepository.AddFundsToWallet(amount, walletId);
    }

}
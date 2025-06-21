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
}
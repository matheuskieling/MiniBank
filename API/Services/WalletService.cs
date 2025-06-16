using API.Models;
using API.Repository;

namespace API.Services;

public class WalletService(WalletRepository walletRepository)
{
    public async Task<Wallet?> GetWallet(Guid id)
    {
        return await walletRepository.GetWallet(id);
    }
}
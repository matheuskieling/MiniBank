using API.Models;
using Core.Data;

namespace API.Repository.Interfaces;

public interface IWalletRepository
{
    Task<Wallet?> GetWalletById(Guid id);
    Task<Wallet?> GetWalletByUserId(Guid id);
    Task<CommandResult<Wallet>> CreateWallet(Guid userId);
}
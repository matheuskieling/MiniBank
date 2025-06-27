using API.Data.Command;
using API.Models;
using API.Repository.Interfaces;
using Core.Data;
using Core.Database;
using Core.Domain;
using Dapper;

namespace API.Repository;

public class WalletRepository : BaseRepository<WalletRepository>, IWalletRepository
{
    public WalletRepository(DbSession dbSession, ILogger<WalletRepository> logger) : base(dbSession, logger) { }

    public async Task<Wallet?> GetWalletById(Guid id)
    {
        const string query = @"
                                SELECT 
                                    id AS Id, 
                                    balance AS Balance, 
                                    user_id as UserId,
                                    created_at AS CreatedAt, 
                                    updated_at AS UpdatedAt
                                FROM wallets 
                                WHERE id = @Id";
        
        object param = new { Id = id };
        
        return await QueryFirstOrDefaultAsync<Wallet>(query, param);
    }
    

    public async Task<Wallet?> GetWalletByUserId(Guid id)
    {
        const string query = @"
                                SELECT 
                                    id AS Id, 
                                    balance AS Balance, 
                                    created_at AS CreatedAt, 
                                    user_id as UserId,
                                    updated_at AS UpdatedAt
                                FROM bank.wallets
                                WHERE user_id = @Id";
        
        object? param = new { Id = id };
        
        return await QueryFirstOrDefaultAsync<Wallet>(query, param);
    }
    
    public async Task<IEnumerable<Wallet>> GetWallets()
    {
        const string query = @"
                                SELECT 
                                    id AS Id, 
                                    balance AS Balance, 
                                    created_at AS CreatedAt, 
                                    user_id as UserId,
                                    updated_at AS UpdatedAt
                                FROM bank.wallets";
        
        return await QueryAsync<Wallet>(query);
    }

    public async Task<CommandResult<Wallet>> CreateWallet(Guid userId)
    {
        var command = new CreateWalletCommand(userId);
        var result = await ExecuteCommand<Wallet>(command);
        return result;
    }
    
    public async Task<bool> AddFundsToWallet(long amount, Guid walletId)
    {
        var command = new AddFundsToWalletCommand(amount, walletId);
        var result = await ExecuteCommand<dynamic>(command);
        return result.Succeded;
    }
    
    public async Task<bool> RemoveFundsFromWallet(long amount, Guid walletId)
    {
        var command = new RemoveFundsFromWalletCommand(amount, walletId);
        var result = await ExecuteCommand<dynamic>(command);
        return result.Succeded;
    }
}
using API.Models;
using Core.Database;
using Core.Domain;
using Dapper;

namespace API.Repository;

public class WalletRepository : BaseRepository<WalletRepository>
{
    public WalletRepository(DbSession dbSession, ILogger<WalletRepository> logger) : base(dbSession, logger) { }

    public async Task<Wallet?> GetWallet(Guid id)
    {
        const string query = @"
                                SELECT 
                                    id AS Id, 
                                    balance AS Balance, 
                                    created_at AS CreatedAt, 
                                    updated_at AS UpdatedAt
                                FROM wallet 
                                WHERE id = @Id";
        
        object param = new { Id = id };
        
        return await QueryFirstOrDefaultAsync<Wallet>(query, param);
    }
}
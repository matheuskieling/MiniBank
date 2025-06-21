using API.Models;
using Core.Data;

namespace API.Data.Command;

public class CreateWalletCommand : BaseCommand
{
    public Guid UserId { get; set; }
    
    public CreateWalletCommand(Guid userId)
    {
        UserId = userId;
    }
    
    public override string Script => @"
        INSERT INTO bank.wallets (user_id)
        VALUES (@UserId)
        RETURNING id as Id,
         user_id as UserId,
         balance as Balanced,
         created_at as CreatedAt,
         updated_at as UpdatedAt
    ";
    public override object Param => new { UserId };
}
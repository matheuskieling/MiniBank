using API.Models;
using Core.Data;

namespace API.Data.Command;

public class AddFundsToWalletCommand : BaseCommand
{
    private long Amount { get; set; }
    private Guid WalletId { get; set; }
    
    public AddFundsToWalletCommand(long amount, Guid walletId)
    {
        Amount = amount;
        WalletId = walletId;
    }
    
    public override string Script => @"
            UPDATE wallets 
            SET balance = balance + @Amount
            WHERE id = @WalletId
    ";
    public override object Param => new { Amount, WalletId };
}
using API.Models;
using Core.Data;

namespace API.Data.Command;

public class RemoveFundsFromWalletCommand : BaseCommand
{
    private long Amount { get; set; }
    private Guid WalletId { get; set; }
    
    public RemoveFundsFromWalletCommand(long amount, Guid walletId)
    {
        Amount = amount;
        WalletId = walletId;
    }
    
    public override string Script => @"
            UPDATE bank.wallets 
            SET balance = balance - @Amount
            WHERE id = @WalletId
    ";
    public override object Param => new { Amount, WalletId };
}
using API.Models;
using API.Repository.Interfaces;
using API.Services.Interfaces;
using Core.Data;

namespace API.Services;

public class TransactionService(IWalletService walletService, ITransactionRepository repository) : ITransactionService
{
    public async Task<CommandResult<Transaction>> CreateTransaction(Guid receiverWalletId, long amount)
    {
        using (var trans = repository.dbSession.Connection.BeginTransaction())
        {
            try
            {
                repository.dbSession.Transaction = trans;
                var receiverWallet = await walletService.GetWalletById(receiverWalletId);
                var senderWallet = await walletService.GetCurrentUserWallet();
                ValidateTransaction(senderWallet, receiverWallet, amount);

                var transaction = new Transaction
                {
                    SenderId = senderWallet!.Id,
                    ReceiverId = receiverWallet!.Id,
                    Amount = amount,
                };
                await walletService.RemoveFundsFromWallet(amount);
                await walletService.AddFundsToWallet(receiverWalletId, amount);
                var result = await repository.CreateTransaction(transaction);
                
                trans.Commit();
                return result;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw;
            }
        }
    }


    private static void ValidateTransaction(Wallet? senderWallet, Wallet? receiverWallet, long amount)
    {
        if (receiverWallet is null || senderWallet is null)
        {
            throw new WalletNotFoundException("Sender or receiver wallets are not found");
        }

        if (receiverWallet.Id == senderWallet.Id)
        {
            throw new UnprocessableEntityException("Cannot send money to the same wallet");
        }
        if (senderWallet.Balance < amount)
        {
            throw new InsufficientFundsException("Insufficient funds to complete the transaction");
        }
        
    }
}
using API.Models;
using Core.Data;

namespace API.Data.Command;

public class CreateTransactionCommand : BaseCommand
{
    public Transaction Transaction { get; set; }
    
    public CreateTransactionCommand(Transaction transaction)
    {
        Transaction = transaction;
    }
    
    public override string Script => @"
        INSERT INTO transactions (sender_id, receiver_id, amount)
        VALUES (@SenderID, @ReceiverId, @Amount)
        RETURNING id as Id,
         sender_id as SenderId,
         receiver_id as ReceiverId,
         amount as Amount,
         created_at as CreatedAt
    ";
    public override object Param => new { SenderId = Transaction.SenderId, ReceiverId = Transaction.ReceiverId, Amount = Transaction.Amount };
}
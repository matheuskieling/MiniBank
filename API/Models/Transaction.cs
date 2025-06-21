namespace API.Models;

public class Transaction
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public long Amount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
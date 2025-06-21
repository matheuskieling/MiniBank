namespace API;

public class WalletNotFoundException : Exception
{
    public WalletNotFoundException(string message) : base(message) { }
    public WalletNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}
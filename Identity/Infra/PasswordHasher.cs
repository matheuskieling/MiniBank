using System.Security.Cryptography;
using System.Text;

namespace Identity.Infra;

public static class PasswordHasher
{
    public static (string Hash, string Salt) HashPassword(string password)
    {
        using var hmac = new HMACSHA256();
        var salt = Convert.ToBase64String(hmac.Key);
        var hash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        return (hash, salt);
    }

    public static bool VerifyPassword(string password, string hash, string salt)
    {
        using var hmac = new HMACSHA256(Convert.FromBase64String(salt));
        var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        return computedHash == hash;
    }

}
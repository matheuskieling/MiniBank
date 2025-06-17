using Identity.Models;

namespace Identity.Services.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}
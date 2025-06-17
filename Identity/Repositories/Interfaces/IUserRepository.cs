using Core.Data;
using Identity.Models;
using Identity.Models.DTO;

namespace Identity.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByUsername(string username);
    Task<CommandResult<UserResponseDto>> CreateUser(string username, string hash, string salt);
}
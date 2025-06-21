using Core.Data;
using Identity.Models;
using Identity.Models.DTO;

namespace Identity.Services.Interfaces;

public interface IUserService
{
    Task<User?> GetUserByUsername(string username);
    Task<bool> DeleteUserById(Guid id);
    Task<User?> AuthenticateUser(AuthRequest authRequest);
    Task<CommandResult<UserResponseDto>> CreateUser(AuthRequest authRequest);
}
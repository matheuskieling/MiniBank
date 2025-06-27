using System.Data;
using Core.Data;
using Identity.Infra;
using Identity.Models;
using Identity.Models.DTO;
using Identity.Repositories.Interfaces;
using Identity.Services.Interfaces;

namespace Identity.Services;

public class UserService(IUserRepository repository) : IUserService
{
    public async Task<User?> GetUserByUsername(string username)
    {
        return await repository.GetUserByUsername(username);
    }

    public async Task<User?> AuthenticateUser(AuthRequest authRequest)
    {
        var user = await GetUserByUsername(authRequest.UserName);
        if (user == null || !PasswordHasher.VerifyPassword(authRequest.Password, user.Hash, user.Salt))
        {
            return null;
        }
        return user;
    }

    public async Task<CommandResult<UserResponseDto>> CreateUser(AuthRequest authRequest)
    {
        var user = await GetUserByUsername(authRequest.UserName);
        if (user != null)
        {
            throw new DuplicateNameException();
        }
        var (hash, salt) = PasswordHasher.HashPassword(authRequest.Password);
        var commandResult = await repository.CreateUser(authRequest.UserName, hash, salt);
        return commandResult;
    }

    public async Task<bool> DeleteUserById(Guid id)
    {
        var user = await repository.GetUserById(id);
        if (user is null)
        {
            return false;
        }
        return await repository.DeleteUserById(id);
    }
}
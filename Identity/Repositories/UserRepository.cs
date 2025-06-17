using Core.Data;
using Core.Database;
using Core.Domain;
using Identity.Data.Commands;
using Identity.Models;
using Identity.Models.DTO;
using Identity.Repositories.Interfaces;

namespace Identity.Repositories;

public class UserRepository(DbSession session, ILogger<UserRepository> logger) :  BaseRepository<UserRepository>(session, logger), IUserRepository
{
    public async Task<User?> GetUserByUsername(string username)
    {
        const string query = @"
                                SELECT 
                                    id AS Id, 
                                    username AS Username, 
                                    salt As Salt,
                                    hash as Hash,
                                    created_at AS CreatedAt, 
                                    updated_at AS UpdatedAt
                                FROM users 
                                WHERE username = @Username";

        object? param = new { Username = username };

        return await QueryFirstOrDefaultAsync<User>(query, param);
    }

    public async Task<CommandResult<UserResponseDto>> CreateUser(string username, string hash, string salt)
    {
        var command = new CreateUserCommand(
            username, hash, salt
        );

        return await ExecuteCommand<UserResponseDto>(command);
    }
}
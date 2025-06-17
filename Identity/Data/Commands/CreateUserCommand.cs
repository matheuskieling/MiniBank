using Core.Data;

namespace Identity.Data.Commands;

public class CreateUserCommand : BaseCommand
{
    private string Username { get; set; }
    private string Hash { get; set; }
    private string Salt { get; set; }
    
    public CreateUserCommand(string username, string hash, string salt)
    {
        Username = username;
        Hash = hash;
        Salt = salt;
    }
    
    
    public override string Script => @"
                                INSERT INTO identity.users (username, hash, salt)
                                VALUES (@Username, @Hash, @Salt)
                                RETURNING 
                                    id AS Id,
                                    username AS Username,
                                    created_at AS CreatedAt,
                                    updated_at AS UpdatedAt";
    
    public override object Param => new { Username, Hash, Salt };
}
using Core.Data;

namespace Identity.Data.Commands;

public class DeleteUserCommand : BaseCommand
{
    private Guid Id { get; set; }
    
    public DeleteUserCommand(Guid id)
    {
        Id = id;
    }
    public override string Script => "DETELE FROM identity.users WHERE id = @Id";
    public override object Param => new { Id };
}
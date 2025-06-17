using System.ComponentModel.DataAnnotations;

namespace Core.Data;

public class CommandResult<T>
{
    public CommandResult(T? data)
    {
        Data = data;
        Succeded = true;
    }

    public CommandResult(string errorMessage, string commandName)
    {
        var error = new ValidationResult(errorMessage, new List<string>() { commandName }) ;
        Errors = new List<ValidationResult> { error };
        Succeded = false;
    }

    public CommandResult(List<ValidationResult> validationMessages)
    {
        Errors = validationMessages;
        Succeded = false;
    }
    
    public bool Succeded { get; private set; }
    public T? Data { get; private set; }
    public List<ValidationResult> Errors { get; private set; } = [];
    
}
using System.ComponentModel.DataAnnotations;

namespace Core.Data;

public abstract class BaseCommand : IBaseCommand
{
    public abstract string Script { get; }
    public abstract object Param { get; }
    public List<ValidationResult> ValidationResults { get; set; } = [];


    public bool IsValid()
    {
        var validationContext = new ValidationContext(this, serviceProvider: null, items: null);
        var validationResults = new List<ValidationResult>();
        if (!Validator.TryValidateObject(this, validationContext, validationResults, validateAllProperties: true))
        {
            ValidationResults = validationResults;
        }
        return !validationResults.Any();
    }
}
namespace ArchyTECH.Core.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(ValidationType type, string message) : base(message)
    {
        Type = type;
    }

    public ValidationType Type { get; set; }


    public static ValidationException NotFound(string message) => new(ValidationType.NotFound, message);
    public static ValidationException MultipleFound(string message) => new(ValidationType.MultipleFound, message);
    public static ValidationException Duplicate(string message) => new(ValidationType.Duplicate, message);
    public static ValidationException InvalidFormat(string message) => new(ValidationType.InvalidFormat, message);
    public static ValidationException Required(string message) => new(ValidationType.Required, message);
}
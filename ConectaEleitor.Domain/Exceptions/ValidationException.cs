namespace ConectaEleitor.Domain.Exceptions;

public class ValidationException : AppException
{
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public ValidationException(string message = "Erro de validação.")
        : base(message, 400)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IReadOnlyDictionary<string, string[]> errors)
        : base("Erro de validação.", 400)
    {
        Errors = errors;
    }
}
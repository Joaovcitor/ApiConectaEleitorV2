namespace ConectaEleitor.Domain.Exceptions;

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message = "Não autorizado.")
        : base(message, 401)
    {
    }

    public UnauthorizedException(string message, int statusCode)
        : base(message, statusCode)
    {
    }
}
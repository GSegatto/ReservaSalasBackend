namespace ReservaSalas.Api.Exceptions;

public abstract class AppException : Exception
{
    public int StatusCode { get; }
    protected AppException(string mensagem, int statusCode) : base(mensagem) => StatusCode = statusCode;
}

public class NegocioException : AppException
{
    public NegocioException(string mensagem) : base(mensagem, StatusCodes.Status400BadRequest) { }
}

public class NaoEncontradoException : AppException
{
    public NaoEncontradoException(string mensagem) : base(mensagem, StatusCodes.Status404NotFound) { }
}

public class ConflitoException : AppException
{
    public ConflitoException(string mensagem) : base(mensagem, StatusCodes.Status409Conflict) { }
}
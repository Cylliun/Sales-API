namespace SalesApi.Middlewares;

public abstract class AppException : Exception
{

    public int StatusCode { get; }

    protected AppException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}

public class ValidationException : AppException
{
    public ValidationException(string message) : base(message, 400)
    {
    }
}

public class NotFoundException : AppException
{
    public NotFoundException(string message) : base(message, 404)
    {
    }
}


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Nixon.ValueUnion;

public enum ErrorType : byte
{
    NotFound,
    ClientError,
    Forbidden
}

public sealed record ErrorValue(ErrorType ErrorType, string? Message)
{
    internal readonly Dictionary<string, object?> AdditionalValues = [];

    public ErrorValue WithValue(string key, object data)
    {
        AdditionalValues[key] = data;
        return this;
    }

    public static ErrorValue NotFoundError(string? message = null)
    {
        return new ErrorValue(ErrorType.NotFound, message);
    }

    public static ErrorValue ClientError(string? message = null)
    {
        return new ErrorValue(ErrorType.ClientError, message);
    }

    public static ErrorValue Forbidden(string? message = null)
    {
        return new ErrorValue(ErrorType.Forbidden, message);
    }

    internal IResult ToResult()
    {
        return Results.Problem(
            detail: Message,
            extensions: AdditionalValues,
            statusCode: ErrorType switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.ClientError => StatusCodes.Status400BadRequest,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            });
    }
    

}
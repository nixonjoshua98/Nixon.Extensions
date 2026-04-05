using Microsoft.AspNetCore.Http;

namespace Nixon.ValueUnion;

public sealed class EndpointValue<TValue> : ValueOr<TValue, ErrorValue>
{
    public EndpointValue(TValue value) : base(value)
    {
        
    }

    private EndpointValue(ErrorValue problem) : base(problem)
    {
        
    }

    public IResult ToResult()
    {
        return TryGetValue(out var value, out var error)
            ? value as IResult ?? Results.Ok(value)
            : Results.Problem(
                detail: error.Message, 
                extensions: error.AdditionalValues,
                statusCode: error.ErrorType switch
                {
                    ErrorType.NotFound => StatusCodes.Status404NotFound,
                    ErrorType.ClientError => StatusCodes.Status400BadRequest,
                    ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                    _ => StatusCodes.Status500InternalServerError
                });
    }


    public static implicit operator EndpointValue<TValue>(TValue value)
    {
        return new EndpointValue<TValue>(value);
    }

    public static implicit operator EndpointValue<TValue>(ErrorValue problem)
    {
        return new EndpointValue<TValue>(problem);
    }
}

public static class ValueOrErrorExtensions
{
    public static async Task<IResult> ToResultAsync<T>(
        this Task<EndpointValue<T>> task,
        CancellationToken cancellationToken = default)
    {
        var result = await task.WaitAsync(cancellationToken);

        return result.ToResult();
    }
}
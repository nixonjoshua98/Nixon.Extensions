using Microsoft.AspNetCore.Http;

namespace Nixon.ValueUnion;

public static class EndpointValueExtensions
{
    public static async Task<IResult> ToResultAsync<T>(
        this Task<EndpointValue<T>> task,
        CancellationToken cancellationToken = default)
    {
        var result = await task.WaitAsync(cancellationToken);

        return result.ToResult();
    }
    
    public static async Task<IResult> ToResultAsync(
        this Task<ErrorValue?> task,
        CancellationToken cancellationToken = default)
    {
        var result = await task.WaitAsync(cancellationToken);

        return result?.ToResult() ?? Results.NoContent();
    }
}
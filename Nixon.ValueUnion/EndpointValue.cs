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
            : error.ToResult();
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
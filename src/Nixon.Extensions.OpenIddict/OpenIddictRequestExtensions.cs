using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using OpenIddict.Abstractions;

namespace Nixon.Extensions.OpenIddict;

public static class OpenIddictRequestExtensions
{
    public static bool TryGetParameterValue<T>(this OpenIddictRequest request, string name, [NotNullWhen(true)] out T? value) 
        where T : IParsable<T>
    {
        return request.TryGetParameter(name, CultureInfo.InvariantCulture, out value);
    }
    
    public static bool TryGetParameterValue(
        this OpenIddictRequest request, 
        string name, 
        [NotNullWhen(true)] out string? value) 
    {
        value = null;

        if (!request.TryGetParameter(name, out var param))
        {
            return false;
        }
            
        value = param.ToString();

        return !string.IsNullOrWhiteSpace(value);
    }  

    public static bool TryGetParameter<T>(
        this OpenIddictRequest request, 
        string name, 
        IFormatProvider provider, 
        [NotNullWhen(true)] out T? value) 
        where T : IParsable<T>
    {
        ArgumentNullException.ThrowIfNull(request);
            
        value = default;

        if (!request.TryGetParameter(name, out var param))
        {
            return false;
        }
            
        var strParam = param.ToString();

        return T.TryParse(strParam, provider, out value);
    }   
}
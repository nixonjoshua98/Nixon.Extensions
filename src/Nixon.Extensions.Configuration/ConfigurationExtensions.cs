using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace Nixon.Extensions.Configuration;

public static class ConfigurationExtensions
{
    extension(IConfiguration configuration)
    {
        public T GetRequiredSection<T>(string sectionName) where T : class, new()
        {
            T instance = new();

            var section = configuration.GetRequiredSection(sectionName);

            section.Bind(instance);

            return instance;
        }

        public bool TryGetValue<T>(string key, [MaybeNullWhen(false)] out T value)
            where T : IParsable<T>
        {
            return T.TryParse(configuration[key], CultureInfo.InvariantCulture, out value);
        }

        public T GetValueOrDefault<T>(string key, T defaultValue = default!) where T : IParsable<T>
        {
            var value = configuration[key];

            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return T.TryParse(value, CultureInfo.InvariantCulture, out var parsedValue) ? 
                parsedValue : 
                throw new Exception($"Configuration key '{key}' was found but failed to be parsed");
        }

        public string GetRequiredConnectionString(string key) => 
            configuration.GetRequiredValue($"ConnectionStrings:{key}");

        public string GetRequiredValue(string key) =>
            configuration[key] ?? throw new Exception($"Configuration key missing '{key}'");
    }
}
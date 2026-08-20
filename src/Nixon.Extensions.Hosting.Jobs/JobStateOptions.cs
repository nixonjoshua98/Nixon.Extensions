namespace Nixon.Extensions.Hosting.Jobs;

internal sealed class JobStateOptions
{
    public readonly Dictionary<string, IConfiguredJob> Jobs = [];
}
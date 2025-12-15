using Microsoft.Extensions.DependencyInjection;
using TopGG.Client;

namespace TopGG.Extensions;

/// <summary>
/// Extension methods for adding Top.gg client to dependency injection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the Top.gg client to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="token">Your Top.gg API token.</param>
    /// <param name="botId">Your bot's Discord ID (required for posting stats and getting votes).</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddTopGGClient(
        this IServiceCollection services,
        string token,
        ulong? botId = null)
    {
        services.AddHttpClient<ITopGGClient, TopGGClient>((httpClient, _) =>
            new TopGGClient(httpClient, token, botId));

        return services;
    }

    /// <summary>
    /// Adds the Top.gg client to the service collection with custom HttpClient configuration.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="token">Your Top.gg API token.</param>
    /// <param name="botId">Your bot's Discord ID (required for posting stats and getting votes).</param>
    /// <param name="configureClient">Action to configure the HttpClient.</param>
    /// <returns>The IHttpClientBuilder for further configuration.</returns>
    public static IHttpClientBuilder AddTopGGClient(
        this IServiceCollection services,
        string token,
        ulong? botId,
        Action<HttpClient> configureClient)
    {
        return services.AddHttpClient<ITopGGClient, TopGGClient>((httpClient, _) =>
        {
            configureClient(httpClient);
            return new TopGGClient(httpClient, token, botId);
        });
    }

    /// <summary>
    /// Adds the Top.gg client to the service collection with custom HttpClient configuration.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="token">Your Top.gg API token.</param>
    /// <param name="configureClient">Action to configure the HttpClient.</param>
    /// <returns>The IHttpClientBuilder for further configuration.</returns>
    public static IHttpClientBuilder AddTopGGClient(
        this IServiceCollection services,
        string token,
        Action<HttpClient> configureClient)
    {
        return services.AddTopGGClient(token, null, configureClient);
    }
}

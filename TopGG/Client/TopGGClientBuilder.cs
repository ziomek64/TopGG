namespace TopGG.Client;

/// <summary>
/// Fluent builder for creating a TopGGClient.
/// </summary>
public class TopGGClientBuilder
{
    private string? _token;
    private ulong? _botId;
    private HttpClient? _httpClient;
    private Action<HttpClient>? _configureHttpClient;

    /// <summary>
    /// Gets the configured bot ID (useful for examples that need the ID separately).
    /// </summary>
    public ulong? BotId => _botId;

    /// <summary>
    /// Creates a new builder instance.
    /// </summary>
    public static TopGGClientBuilder Create() => new();

    /// <summary>
    /// Sets the Top.gg API token.
    /// </summary>
    /// <param name="token">Your Top.gg API token.</param>
    /// <returns>The builder.</returns>
    public TopGGClientBuilder WithToken(string token)
    {
        _token = token;
        return this;
    }

    /// <summary>
    /// Sets the bot's Discord ID.
    /// </summary>
    /// <param name="botId">Your bot's Discord ID.</param>
    /// <returns>The builder.</returns>
    public TopGGClientBuilder WithBotId(ulong botId)
    {
        _botId = botId;
        return this;
    }

    /// <summary>
    /// Uses a custom HttpClient.
    /// </summary>
    /// <param name="httpClient">The HttpClient to use.</param>
    /// <returns>The builder.</returns>
    public TopGGClientBuilder WithHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        return this;
    }

    /// <summary>
    /// Configures the HttpClient (only used when not providing a custom HttpClient).
    /// </summary>
    /// <param name="configure">Action to configure the HttpClient.</param>
    /// <returns>The builder.</returns>
    public TopGGClientBuilder ConfigureHttpClient(Action<HttpClient> configure)
    {
        _configureHttpClient = configure;
        return this;
    }

    /// <summary>
    /// Reads the token from an environment variable.
    /// </summary>
    /// <param name="variableName">The environment variable name (default: TOPGG_TOKEN).</param>
    /// <returns>The builder.</returns>
    public TopGGClientBuilder WithTokenFromEnvironment(string variableName = "TOPGG_TOKEN")
    {
        _token = Environment.GetEnvironmentVariable(variableName);
        return this;
    }

    /// <summary>
    /// Reads the bot ID from an environment variable.
    /// </summary>
    /// <param name="variableName">The environment variable name (default: TOPGG_BOT_ID).</param>
    /// <returns>The builder.</returns>
    public TopGGClientBuilder WithBotIdFromEnvironment(string variableName = "TOPGG_BOT_ID")
    {
        var value = Environment.GetEnvironmentVariable(variableName);
        if (ulong.TryParse(value, out var botId))
            _botId = botId;
        return this;
    }

    /// <summary>
    /// Builds the TopGGClient.
    /// </summary>
    /// <returns>A configured TopGGClient.</returns>
    /// <exception cref="InvalidOperationException">Thrown when required configuration is missing.</exception>
    public TopGGClient Build()
    {
        if (string.IsNullOrEmpty(_token))
            throw new InvalidOperationException("Token is required. Use WithToken() or WithTokenFromEnvironment().");

        if (_httpClient != null)
        {
            return new TopGGClient(_httpClient, _token, _botId);
        }

        var client = new TopGGClient(_token, _botId);

        if (_configureHttpClient != null)
        {
            // Note: This only works if we expose the HttpClient,
            // for now we'll create a new one with configuration
            var httpClient = new HttpClient();
            _configureHttpClient(httpClient);
            return new TopGGClient(httpClient, _token, _botId);
        }

        return client;
    }

    /// <summary>
    /// Tries to build the TopGGClient, returning null if configuration is incomplete.
    /// </summary>
    /// <param name="client">The built client, or null if configuration is incomplete.</param>
    /// <returns>True if the client was built successfully.</returns>
    public bool TryBuild(out TopGGClient? client)
    {
        if (string.IsNullOrEmpty(_token))
        {
            client = null;
            return false;
        }

        client = Build();
        return true;
    }
}

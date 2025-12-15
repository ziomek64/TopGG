using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using TopGG.Enums;
using TopGG.Exceptions;
using TopGG.Models;
using TopGG.Models.Discord;

namespace TopGG.Client;

/// <summary>
/// Client for interacting with the Top.gg API.
/// </summary>
public class TopGGClient : ITopGGClient
{
    private const string BaseUrlV0 = "https://top.gg/api";
    private const string BaseUrlV1 = "https://top.gg/api/v1";

    private readonly HttpClient _httpClient;
    private readonly string _token;
    private readonly ulong? _botId;
    private readonly bool _disposeHttpClient;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    /// <summary>
    /// Creates a new Top.gg client with the specified token.
    /// </summary>
    /// <param name="token">Your Top.gg API token.</param>
    /// <param name="botId">Your bot's Discord ID (required for posting stats and getting votes).</param>
    public TopGGClient(string token, ulong? botId = null)
    {
        _token = token ?? throw new ArgumentNullException(nameof(token));
        _botId = botId;
        _httpClient = new HttpClient();
        _disposeHttpClient = true;
    }

    /// <summary>
    /// Creates a new Top.gg client with a custom HttpClient.
    /// </summary>
    /// <param name="httpClient">The HttpClient to use.</param>
    /// <param name="token">Your Top.gg API token.</param>
    /// <param name="botId">Your bot's Discord ID (required for posting stats and getting votes).</param>
    public TopGGClient(HttpClient httpClient, string token, ulong? botId = null)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _token = token ?? throw new ArgumentNullException(nameof(token));
        _botId = botId;
        _disposeHttpClient = false;
    }

    #region Bot Endpoints (v0)

    /// <inheritdoc />
    public async Task<BotSearchResult> SearchBotsAsync(
        int? limit = null,
        int? offset = null,
        string? sort = null,
        string? fields = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new List<string>();

        if (limit.HasValue)
            queryParams.Add($"limit={limit.Value}");
        if (offset.HasValue)
            queryParams.Add($"offset={offset.Value}");
        if (!string.IsNullOrEmpty(sort))
            queryParams.Add($"sort={Uri.EscapeDataString(sort)}");
        if (!string.IsNullOrEmpty(fields))
            queryParams.Add($"fields={Uri.EscapeDataString(fields)}");

        var query = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
        return await GetAsync<BotSearchResult>($"{BaseUrlV0}/bots{query}", cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<BotStats> GetBotStatsAsync(CancellationToken cancellationToken = default)
    {
        EnsureBotId();
        return await GetAsync<BotStats>($"{BaseUrlV0}/bots/{_botId}/stats", cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task PostBotStatsAsync(
        int guildCount,
        CancellationToken cancellationToken = default)
    {
        EnsureBotId();

        var stats = new BotStatsPost
        {
            ServerCount = guildCount
        };

        await PostAsync($"{BaseUrlV0}/bots/{_botId}/stats", stats, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<Vote[]> GetBotVotesAsync(CancellationToken cancellationToken = default)
    {
        EnsureBotId();
        return await GetAsync<Vote[]>($"{BaseUrlV0}/bots/{_botId}/votes", cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<VoteCheck> CheckUserVoteAsync(ulong userId, CancellationToken cancellationToken = default)
    {
        EnsureBotId();
        return await GetAsync<VoteCheck>($"{BaseUrlV0}/bots/{_botId}/check?userId={userId}", cancellationToken).ConfigureAwait(false);
    }

    #endregion

    #region Project Endpoints (v1)

    /// <inheritdoc />
    public async Task UpdateBotCommandsAsync(ApplicationCommand[] commands, CancellationToken cancellationToken = default)
    {
        if (commands == null)
            throw new ArgumentNullException(nameof(commands));

        await PostAsyncV1($"{BaseUrlV1}/projects/@me/commands", commands, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<VoteStatus> GetVoteStatusAsync(ulong userId, string? source = null, CancellationToken cancellationToken = default)
    {
        var query = !string.IsNullOrEmpty(source) ? $"?source={Uri.EscapeDataString(source)}" : "";
        return await GetAsyncV1<VoteStatus>($"{BaseUrlV1}/projects/@me/votes/{userId}{query}", cancellationToken).ConfigureAwait(false);
    }

    #endregion

    #region Fluent API

    /// <inheritdoc />
    public BotSearchBuilder Search() => new(this);

    #endregion

    #region Private Helpers

    private void EnsureBotId()
    {
        if (!_botId.HasValue)
            throw new InvalidOperationException("Bot ID must be specified to use this endpoint. Pass the botId parameter to the constructor.");
    }

    private async Task<T> GetAsync<T>(string url, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.TryAddWithoutValidation("Authorization", _token);

        return await SendRequestAsync<T>(request, cancellationToken).ConfigureAwait(false);
    }

    private async Task<T> GetAsyncV1<T>(string url, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

        return await SendRequestAsync<T>(request, cancellationToken).ConfigureAwait(false);
    }

    private async Task PostAsync<T>(string url, T data, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.TryAddWithoutValidation("Authorization", _token);
        request.Content = new StringContent(
            JsonSerializer.Serialize(data, JsonOptions),
            Encoding.UTF8,
            "application/json");

        await SendRequestAsync(request, cancellationToken).ConfigureAwait(false);
    }

    private async Task PostAsyncV1<T>(string url, T data, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        request.Content = new StringContent(
            JsonSerializer.Serialize(data, JsonOptions),
            Encoding.UTF8,
            "application/json");

        await SendRequestAsync(request, cancellationToken).ConfigureAwait(false);
    }

    private async Task<T> SendRequestAsync<T>(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            throw new TopGGException("Failed to send request to Top.gg API", ex);
        }

        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        await HandleResponseErrorsAsync(response, content).ConfigureAwait(false);

        try
        {
            return JsonSerializer.Deserialize<T>(content, JsonOptions)
                   ?? throw new TopGGException("Failed to deserialize response from Top.gg API");
        }
        catch (JsonException ex)
        {
            throw new TopGGException($"Failed to deserialize response from Top.gg API: {content}", ex);
        }
    }

    private async Task SendRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            throw new TopGGException("Failed to send request to Top.gg API", ex);
        }

        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        await HandleResponseErrorsAsync(response, content).ConfigureAwait(false);
    }

    private static Task HandleResponseErrorsAsync(HttpResponseMessage response, string content)
    {
        if (response.IsSuccessStatusCode)
            return Task.CompletedTask;

        var statusCode = response.StatusCode;
        var errorMessage = ParseErrorMessage(content);

        switch (statusCode)
        {
            case HttpStatusCode.NotFound:
                throw new TopGGNotFoundException(errorMessage);

            case (HttpStatusCode)429: // TooManyRequests
                var retryAfter = ParseRetryAfter(response, content);
                throw new TopGGRateLimitException(errorMessage, retryAfter);

            case >= HttpStatusCode.InternalServerError:
                throw new TopGGServerException(errorMessage, statusCode);

            case >= HttpStatusCode.BadRequest:
                throw new TopGGBadRequestException(errorMessage, statusCode);

            default:
                throw new TopGGException($"Unexpected error ({(int)statusCode}): {errorMessage}");
        }
    }

    private static string ParseErrorMessage(string content)
    {
        if (string.IsNullOrEmpty(content))
            return "Unknown error";

        try
        {
            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(content, JsonOptions);
            if (problemDetails?.Detail != null)
            {
                return problemDetails.Title != null
                    ? $"{problemDetails.Title}: {problemDetails.Detail}"
                    : problemDetails.Detail;
            }
        }
        catch
        {
            // Not a ProblemDetails response, use raw content
        }

        return content;
    }

    private static TimeSpan? ParseRetryAfter(HttpResponseMessage response, string content)
    {
        // Try HTTP header first
        if (response.Headers.RetryAfter?.Delta != null)
            return response.Headers.RetryAfter.Delta;

        // Try parsing from JSON body: {"retry-after": 3600}
        if (!string.IsNullOrEmpty(content))
        {
            try
            {
                using var doc = JsonDocument.Parse(content);
                if (doc.RootElement.TryGetProperty("retry-after", out var retryAfterElement))
                {
                    if (retryAfterElement.TryGetInt32(out var seconds))
                        return TimeSpan.FromSeconds(seconds);
                }
            }
            catch
            {
                // Not valid JSON, ignore
            }
        }

        return null;
    }

    #endregion

    /// <inheritdoc />
    public void Dispose()
    {
        if (_disposeHttpClient)
            _httpClient.Dispose();
    }
}

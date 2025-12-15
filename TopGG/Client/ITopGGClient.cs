using TopGG.Enums;
using TopGG.Models;
using TopGG.Models.Discord;

namespace TopGG.Client;

/// <summary>
/// Interface for the Top.gg API client.
/// </summary>
public interface ITopGGClient : IDisposable
{
    #region Bot Endpoints (v0)

    /// <summary>
    /// Searches for bots on Top.gg.
    /// </summary>
    /// <param name="limit">The number of bots to return (max 500, default 50).</param>
    /// <param name="offset">The number of bots to skip.</param>
    /// <param name="sort">The field to sort by. Prefix with '-' to reverse.</param>
    /// <param name="fields">Comma-separated list of fields to return.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A search result containing bots.</returns>
    Task<BotSearchResult> SearchBotsAsync(
        int? limit = null,
        int? offset = null,
        string? sort = null,
        string? fields = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the statistics for your bot.
    /// </summary>
    /// <remarks>
    /// This endpoint only returns statistics for your own bot (the one configured in the client).
    /// It cannot be used to retrieve statistics for other bots.
    /// </remarks>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The bot's statistics.</returns>
    Task<BotStats> GetBotStatsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Posts statistics for your bot.
    /// </summary>
    /// <param name="guildCount">The number of guilds (servers) the bot is in.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task PostBotStatsAsync(
        int guildCount,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the last 1000 votes for your bot.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Array of votes.</returns>
    Task<Vote[]> GetBotVotesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a user has voted for your bot in the last 12 hours.
    /// </summary>
    /// <param name="userId">The user's Discord ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Vote check result.</returns>
    Task<VoteCheck> CheckUserVoteAsync(ulong userId, CancellationToken cancellationToken = default);

    #endregion

    #region Project Endpoints (v1)

    /// <summary>
    /// Updates the Discord slash commands for your bot on Top.gg.
    /// </summary>
    /// <param name="commands">Array of application commands.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task UpdateBotCommandsAsync(ApplicationCommand[] commands, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the vote status for a user (v1 API).
    /// </summary>
    /// <param name="userId">The user's Discord ID.</param>
    /// <param name="source">Optional source where the user ID is coming from.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The vote status.</returns>
    Task<VoteStatus> GetVoteStatusAsync(ulong userId, string? source = null, CancellationToken cancellationToken = default);

    #endregion

    #region Fluent API

    /// <summary>
    /// Creates a new bot search request builder.
    /// </summary>
    /// <returns>A bot search builder.</returns>
    BotSearchBuilder Search();

    #endregion
}

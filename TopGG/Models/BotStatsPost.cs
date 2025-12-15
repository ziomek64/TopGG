using System.Text.Json.Serialization;

namespace TopGG.Models;

/// <summary>
/// Represents the payload for posting bot statistics.
/// </summary>
public class BotStatsPost
{
    /// <summary>
    /// The amount of servers the bot is in.
    /// Can be a number (total server count) or an array of numbers (acts like shards).
    /// </summary>
    [JsonPropertyName("server_count")]
    public object? ServerCount { get; set; }

    /// <summary>
    /// Array of server counts per shard (optional).
    /// </summary>
    [JsonPropertyName("shards")]
    public List<int>? Shards { get; set; }

    /// <summary>
    /// The zero-indexed ID of the shard posting (optional).
    /// Makes server_count set the shard-specific server count.
    /// </summary>
    [JsonPropertyName("shard_id")]
    public int? ShardId { get; set; }

    /// <summary>
    /// The total amount of shards the bot has (optional).
    /// </summary>
    [JsonPropertyName("shard_count")]
    public int? ShardCount { get; set; }
}

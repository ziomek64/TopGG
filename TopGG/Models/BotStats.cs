using System.Text.Json.Serialization;

namespace TopGG.Models;

/// <summary>
/// Represents a bot's statistics.
/// </summary>
public class BotStats
{
    /// <summary>The amount of servers the bot is in</summary>
    [JsonPropertyName("server_count")]
    public int? ServerCount { get; set; }

    /// <summary>The amount of servers the bot is in per shard. Always present but can be empty.</summary>
    [Obsolete("No longer supported by Top.gg API v0.")]
    [JsonPropertyName("shards")]
    public List<int>? Shards { get; set; }

    /// <summary>The amount of shards the bot has</summary>
    [Obsolete("No longer supported by Top.gg API v0.")]
    [JsonPropertyName("shard_count")]
    public int? ShardCount { get; set; }
}

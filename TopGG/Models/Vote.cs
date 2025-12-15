using System.Text.Json.Serialization;
using TopGG.Json;

namespace TopGG.Models;

/// <summary>
/// Represents a vote for a bot (from the last 1000 votes endpoint).
/// </summary>
public class Vote
{
    /// <summary>The voter's Discord ID</summary>
    [JsonPropertyName("id")]
    [JsonConverter(typeof(SnowflakeConverter))]
    public ulong Id { get; set; }

    /// <summary>The voter's Discord username</summary>
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    /// <summary>The voter's discriminator</summary>
    [Obsolete("No longer supported by Top.gg API v0.")]
    [JsonPropertyName("discriminator")]
    public string Discriminator { get; set; } = string.Empty;

    /// <summary>The voter's avatar URL</summary>
    [JsonPropertyName("avatar")]
    public string? Avatar { get; set; }
}

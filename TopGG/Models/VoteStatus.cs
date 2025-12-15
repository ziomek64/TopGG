using System.Text.Json.Serialization;

namespace TopGG.Models;

/// <summary>
/// Represents the vote status for a user (v1 API).
/// </summary>
public class VoteStatus
{
    /// <summary>The timestamp of when the user last voted</summary>
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>The timestamp of when the user can vote again</summary>
    [JsonPropertyName("expires_at")]
    public DateTime ExpiresAt { get; set; }

    /// <summary>The amount of votes this vote counted for</summary>
    [JsonPropertyName("weight")]
    public int Weight { get; set; }
}

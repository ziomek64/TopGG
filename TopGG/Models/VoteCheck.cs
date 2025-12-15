using System.Text.Json.Serialization;

namespace TopGG.Models;

/// <summary>
/// Represents the result of checking if a user has voted (v0 API).
/// </summary>
public class VoteCheck
{
    /// <summary>Whether the user has voted in the last 12 hours</summary>
    [JsonPropertyName("voted")]
    public int Voted { get; set; }

    /// <summary>Returns true if the user has voted</summary>
    [JsonIgnore]
    public bool HasVoted => Voted == 1;
}

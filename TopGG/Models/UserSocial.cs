using System.Text.Json.Serialization;

namespace TopGG.Models;

/// <summary>
/// Represents a user's social media links.
/// </summary>
public class UserSocial
{
    /// <summary>The user's YouTube channel ID</summary>
    [JsonPropertyName("youtube")]
    public string? Youtube { get; set; }

    /// <summary>The user's Reddit username</summary>
    [JsonPropertyName("reddit")]
    public string? Reddit { get; set; }

    /// <summary>The user's Twitter username</summary>
    [JsonPropertyName("twitter")]
    public string? Twitter { get; set; }

    /// <summary>The user's Instagram username</summary>
    [JsonPropertyName("instagram")]
    public string? Instagram { get; set; }

    /// <summary>The user's GitHub username</summary>
    [JsonPropertyName("github")]
    public string? Github { get; set; }
}

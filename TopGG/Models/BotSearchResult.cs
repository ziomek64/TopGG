using System.Text.Json.Serialization;

namespace TopGG.Models;

/// <summary>
/// Represents the result of a bot search query.
/// </summary>
public class BotSearchResult
{
    /// <summary>Array of bots matching the search</summary>
    [JsonPropertyName("results")]
    public List<Bot> Results { get; set; } = new();

    /// <summary>The limit used in the query</summary>
    [JsonPropertyName("limit")]
    public int Limit { get; set; }

    /// <summary>The offset used in the query</summary>
    [JsonPropertyName("offset")]
    public int Offset { get; set; }

    /// <summary>The count of bots returned</summary>
    [JsonPropertyName("count")]
    public int Count { get; set; }

    /// <summary>The total count of bots matching the query</summary>
    [JsonPropertyName("total")]
    public int Total { get; set; }
}

using System.Text.Json.Serialization;

namespace TopGG.Models;

/// <summary>
/// RFC 7807 Problem Details response from the API.
/// </summary>
public class ProblemDetails
{
    /// <summary>A URI reference that identifies the problem type</summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>A short, human-readable summary of the problem type</summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>The HTTP status code</summary>
    [JsonPropertyName("status")]
    public int? Status { get; set; }

    /// <summary>A human-readable explanation specific to this occurrence of the problem</summary>
    [JsonPropertyName("detail")]
    public string? Detail { get; set; }
}

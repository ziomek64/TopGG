using System.Text.Json.Serialization;

namespace TopGG.Models.Discord;

/// <summary>
/// Represents a choice for an application command option.
/// </summary>
public class ApplicationCommandOptionChoice
{
    /// <summary>1-100 character choice name</summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>Localization dictionary for the name field</summary>
    [JsonPropertyName("name_localizations")]
    public Dictionary<string, string>? NameLocalizations { get; set; }

    /// <summary>Value for the choice, up to 100 characters if string. Type depends on option type.</summary>
    [JsonPropertyName("value")]
    public object Value { get; set; } = string.Empty;
}

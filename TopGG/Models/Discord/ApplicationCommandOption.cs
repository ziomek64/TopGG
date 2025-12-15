using System.Text.Json.Serialization;
using TopGG.Enums.Discord;

namespace TopGG.Models.Discord;

/// <summary>
/// Represents an option for an application command.
/// </summary>
public class ApplicationCommandOption
{
    /// <summary>Type of option</summary>
    [JsonPropertyName("type")]
    public ApplicationCommandOptionType Type { get; set; }

    /// <summary>1-32 character name</summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>Localization dictionary for the name field</summary>
    [JsonPropertyName("name_localizations")]
    public Dictionary<string, string>? NameLocalizations { get; set; }

    /// <summary>1-100 character description</summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>Localization dictionary for the description field</summary>
    [JsonPropertyName("description_localizations")]
    public Dictionary<string, string>? DescriptionLocalizations { get; set; }

    /// <summary>Whether the parameter is required or optional, default false</summary>
    [JsonPropertyName("required")]
    public bool? Required { get; set; }

    /// <summary>Choices for the user to pick from, max 25</summary>
    [JsonPropertyName("choices")]
    public List<ApplicationCommandOptionChoice>? Choices { get; set; }

    /// <summary>Nested options for subcommand or subcommand group types</summary>
    [JsonPropertyName("options")]
    public List<ApplicationCommandOption>? Options { get; set; }

    /// <summary>Channel types to restrict the option to</summary>
    [JsonPropertyName("channel_types")]
    public List<ChannelType>? ChannelTypes { get; set; }

    /// <summary>The minimum value permitted (for INTEGER/NUMBER options)</summary>
    [JsonPropertyName("min_value")]
    public double? MinValue { get; set; }

    /// <summary>The maximum value permitted (for INTEGER/NUMBER options)</summary>
    [JsonPropertyName("max_value")]
    public double? MaxValue { get; set; }

    /// <summary>The minimum allowed length for STRING options (0-6000)</summary>
    [JsonPropertyName("min_length")]
    public int? MinLength { get; set; }

    /// <summary>The maximum allowed length for STRING options (1-6000)</summary>
    [JsonPropertyName("max_length")]
    public int? MaxLength { get; set; }

    /// <summary>If autocomplete interactions are enabled for this option</summary>
    [JsonPropertyName("autocomplete")]
    public bool? Autocomplete { get; set; }
}

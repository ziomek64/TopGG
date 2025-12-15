using System.Text.Json.Serialization;
using TopGG.Enums.Discord;
using TopGG.Json;

namespace TopGG.Models.Discord;

/// <summary>
/// Represents a Discord application command.
/// </summary>
public class ApplicationCommand
{
    /// <summary>Unique ID of command</summary>
    [JsonPropertyName("id")]
    [JsonConverter(typeof(NullableSnowflakeConverter))]
    public ulong? Id { get; set; }

    /// <summary>Type of command, defaults to ChatInput (1)</summary>
    [JsonPropertyName("type")]
    public ApplicationCommandType? Type { get; set; }

    /// <summary>ID of the parent application</summary>
    [JsonPropertyName("application_id")]
    [JsonConverter(typeof(NullableSnowflakeConverter))]
    public ulong? ApplicationId { get; set; }

    /// <summary>Guild ID of the command, if not global</summary>
    [JsonPropertyName("guild_id")]
    [JsonConverter(typeof(NullableSnowflakeConverter))]
    public ulong? GuildId { get; set; }

    /// <summary>Name of command, 1-32 characters</summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>Localization dictionary for name field</summary>
    [JsonPropertyName("name_localizations")]
    public Dictionary<string, string>? NameLocalizations { get; set; }

    /// <summary>Description for CHAT_INPUT commands, 1-100 characters. Empty string for USER and MESSAGE commands</summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>Localization dictionary for description field</summary>
    [JsonPropertyName("description_localizations")]
    public Dictionary<string, string>? DescriptionLocalizations { get; set; }

    /// <summary>Parameters for the command, max of 25 (only for CHAT_INPUT)</summary>
    [JsonPropertyName("options")]
    public List<ApplicationCommandOption>? Options { get; set; }

    /// <summary>Set of permissions represented as a bit set</summary>
    [JsonPropertyName("default_member_permissions")]
    public string? DefaultMemberPermissions { get; set; }

    /// <summary>Deprecated: Indicates whether the command is available in DMs with the app</summary>
    [JsonPropertyName("dm_permission")]
    public bool? DmPermission { get; set; }

    /// <summary>Deprecated: Indicates whether the command is enabled by default when added to a guild</summary>
    [JsonPropertyName("default_permission")]
    public bool? DefaultPermission { get; set; }

    /// <summary>Indicates whether the command is age-restricted, defaults to false</summary>
    [JsonPropertyName("nsfw")]
    public bool? Nsfw { get; set; }

    /// <summary>Installation contexts where the command is available</summary>
    [JsonPropertyName("integration_types")]
    public List<IntegrationType>? IntegrationTypes { get; set; }

    /// <summary>Interaction context(s) where the command can be used</summary>
    [JsonPropertyName("contexts")]
    public List<InteractionContextType>? Contexts { get; set; }

    /// <summary>Autoincrementing version identifier</summary>
    [JsonPropertyName("version")]
    [JsonConverter(typeof(NullableSnowflakeConverter))]
    public ulong? Version { get; set; }

    /// <summary>Handler type for PRIMARY_ENTRY_POINT commands</summary>
    [JsonPropertyName("handler")]
    public EntryPointCommandHandlerType? Handler { get; set; }
}

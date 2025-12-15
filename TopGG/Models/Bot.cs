using System.Text.Json.Serialization;
using TopGG.Json;

namespace TopGG.Models;

/// <summary>
/// Represents a bot on Top.gg.
/// </summary>
public class Bot
{
    /// <summary>The Top.gg ID of the bot</summary>
    [JsonPropertyName("id")]
    [JsonConverter(typeof(SnowflakeConverter))]
    public ulong Id { get; set; }

    /// <summary>The Discord ID of the bot</summary>
    [JsonPropertyName("clientid")]
    [JsonConverter(typeof(NullableSnowflakeConverter))]
    public ulong? ClientId { get; set; }

    /// <summary>The bot's username</summary>
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    /// <summary>The bot's discriminator</summary>
    [Obsolete("No longer supported by Top.gg API v0.")]
    [JsonPropertyName("discriminator")]
    public string Discriminator { get; set; } = string.Empty;

    /// <summary>The bot's avatar hash</summary>
    [JsonPropertyName("avatar")]
    public string? Avatar { get; set; }

    /// <summary>The cdn hash of the bot's avatar if the bot has none</summary>
    [Obsolete("No longer supported by Top.gg API v0.")]
    [JsonPropertyName("defAvatar")]
    public string? DefAvatar { get; set; }

    /// <summary>The URL for the banner image</summary>
    [Obsolete("No longer supported by Top.gg API v0.")]
    [JsonPropertyName("bannerUrl")]
    public string? BannerUrl { get; set; }

    /// <summary>The library the bot was built with</summary>
    [Obsolete("No longer supported by Top.gg API v0.")]
    [JsonPropertyName("lib")]
    public string? Lib { get; set; }

    /// <summary>The bot's prefix</summary>
    [JsonPropertyName("prefix")]
    public string? Prefix { get; set; }

    /// <summary>Short description of the bot</summary>
    [JsonPropertyName("shortdesc")]
    public string? ShortDescription { get; set; }

    /// <summary>Long description of the bot. Can contain HTML and/or Markdown</summary>
    [JsonPropertyName("longdesc")]
    public string? LongDescription { get; set; }

    /// <summary>Tags associated with the bot</summary>
    [JsonPropertyName("tags")]
    public List<string>? Tags { get; set; }

    /// <summary>The bot's website URL</summary>
    [JsonPropertyName("website")]
    public string? Website { get; set; }

    /// <summary>The bot's support server URL</summary>
    [JsonPropertyName("support")]
    public string? Support { get; set; }

    /// <summary>The link to the GitHub repo of the bot</summary>
    [JsonPropertyName("github")]
    public string? Github { get; set; }

    /// <summary>The owners of the bot. First one in the array is the main owner</summary>
    [JsonPropertyName("owners")]
    [JsonConverter(typeof(SnowflakeListConverter))]
    public List<ulong>? Owners { get; set; }

    /// <summary>The guilds featured on the bot page</summary>
    [Obsolete("No longer supported by Top.gg API v0.")]
    [JsonPropertyName("guilds")]
    [JsonConverter(typeof(SnowflakeListConverter))]
    public List<ulong>? Guilds { get; set; }

    /// <summary>The custom bot invite URL</summary>
    [JsonPropertyName("invite")]
    public string? Invite { get; set; }

    /// <summary>The date when the bot was submitted (in ISO 8601)</summary>
    [JsonPropertyName("date")]
    public DateTime? Date { get; set; }

    /// <summary>The certified status of the bot</summary>
    [Obsolete("No longer supported by Top.gg API v0.")]
    [JsonPropertyName("certifiedBot")]
    public bool CertifiedBot { get; set; }

    /// <summary>The bot's vanity URL</summary>
    [JsonPropertyName("vanity")]
    public string? Vanity { get; set; }

    /// <summary>The amount of votes the bot has</summary>
    [JsonPropertyName("points")]
    public int Points { get; set; }

    /// <summary>The amount of votes the bot has this month</summary>
    [JsonPropertyName("monthlyPoints")]
    public int MonthlyPoints { get; set; }

    /// <summary>The guild id for the donatebot setup</summary>
    [Obsolete("No longer supported by Top.gg API v0.")]
    [JsonPropertyName("donatebotguildid")]
    [JsonConverter(typeof(NullableSnowflakeConverter))]
    public ulong? DonateBotGuildId { get; set; }

    /// <summary>The amount of servers the bot is in based on posted stats</summary>
    [JsonPropertyName("server_count")]
    public int? ServerCount { get; set; }

    /// <summary>The amount of shards the bot has</summary>
    [Obsolete("No longer supported by Top.gg API v0.")]
    [JsonPropertyName("shard_count")]
    public int? ShardCount { get; set; }

    /// <summary>The bot's reviews on Top.gg</summary>
    [JsonPropertyName("reviews")]
    public BotReviews? Reviews { get; set; }
}

/// <summary>
/// Represents a bot's review information.
/// </summary>
public class BotReviews
{
    /// <summary>This bot's average review score out of 5</summary>
    [JsonPropertyName("averageScore")]
    public double AverageScore { get; set; }

    /// <summary>This bot's review count</summary>
    [JsonPropertyName("count")]
    public int Count { get; set; }
}

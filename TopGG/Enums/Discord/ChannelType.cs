namespace TopGG.Enums.Discord;

/// <summary>
/// Discord channel types for restricting channel option choices.
/// </summary>
public enum ChannelType
{
    /// <summary>A text channel within a server</summary>
    GuildText = 0,

    /// <summary>A direct message between users</summary>
    Dm = 1,

    /// <summary>A voice channel within a server</summary>
    GuildVoice = 2,

    /// <summary>A direct message between multiple users</summary>
    GroupDm = 3,

    /// <summary>An organizational category that contains up to 50 channels</summary>
    GuildCategory = 4,

    /// <summary>A channel that users can follow and crosspost into their own server</summary>
    GuildAnnouncement = 5,

    /// <summary>A temporary sub-channel within a GUILD_ANNOUNCEMENT channel</summary>
    AnnouncementThread = 10,

    /// <summary>A temporary sub-channel within a GUILD_TEXT or GUILD_FORUM channel</summary>
    PublicThread = 11,

    /// <summary>A temporary sub-channel within a GUILD_TEXT channel with limited access</summary>
    PrivateThread = 12,

    /// <summary>A voice channel for hosting events with an audience</summary>
    GuildStageVoice = 13,

    /// <summary>The channel in a hub containing the listed servers</summary>
    GuildDirectory = 14,

    /// <summary>A channel that can only contain threads</summary>
    GuildForum = 15,

    /// <summary>A channel like a forum but for media-only content</summary>
    GuildMedia = 16
}

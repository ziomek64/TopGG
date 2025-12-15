namespace TopGG.Enums.Discord;

/// <summary>
/// Interaction context types where a command can be used.
/// </summary>
public enum InteractionContextType
{
    /// <summary>Interaction can be used within servers</summary>
    Guild = 0,

    /// <summary>Interaction can be used within DMs with the app's bot user</summary>
    BotDm = 1,

    /// <summary>Interaction can be used within Group DMs and DMs other than the app's bot user</summary>
    PrivateChannel = 2
}

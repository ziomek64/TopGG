namespace TopGG.Enums.Discord;

/// <summary>
/// Installation contexts for application commands.
/// </summary>
public enum IntegrationType
{
    /// <summary>App is installable to servers</summary>
    GuildInstall = 0,

    /// <summary>App is installable to users</summary>
    UserInstall = 1
}

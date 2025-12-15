namespace TopGG.Enums.Discord;

/// <summary>
/// The type of application command option.
/// </summary>
public enum ApplicationCommandOptionType
{
    /// <summary>A subcommand</summary>
    SubCommand = 1,

    /// <summary>A subcommand group</summary>
    SubCommandGroup = 2,

    /// <summary>A string option</summary>
    String = 3,

    /// <summary>Any integer between -2^53+1 and 2^53-1</summary>
    Integer = 4,

    /// <summary>A boolean option</summary>
    Boolean = 5,

    /// <summary>A user option</summary>
    User = 6,

    /// <summary>Includes all channel types + categories</summary>
    Channel = 7,

    /// <summary>A role option</summary>
    Role = 8,

    /// <summary>Includes users and roles</summary>
    Mentionable = 9,

    /// <summary>Any double between -2^53 and 2^53</summary>
    Number = 10,

    /// <summary>An attachment object</summary>
    Attachment = 11
}

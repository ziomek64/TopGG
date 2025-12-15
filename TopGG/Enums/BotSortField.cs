namespace TopGG.Enums;

/// <summary>
/// Fields available for sorting bot search results.
/// </summary>
public enum BotSortField
{
    /// <summary>Sort by bot username (ascending)</summary>
    Username,

    /// <summary>Sort by bot username (descending)</summary>
    UsernameDesc,

    /// <summary>Sort by bot ID (ascending)</summary>
    Id,

    /// <summary>Sort by bot ID (descending)</summary>
    IdDesc,

    /// <summary>Sort by server count (ascending)</summary>
    ServerCount,

    /// <summary>Sort by server count (descending)</summary>
    ServerCountDesc,

    /// <summary>Sort by total points/votes (ascending)</summary>
    Points,

    /// <summary>Sort by total points/votes (descending)</summary>
    PointsDesc,

    /// <summary>Sort by monthly points/votes (ascending)</summary>
    MonthlyPoints,

    /// <summary>Sort by monthly points/votes (descending)</summary>
    MonthlyPointsDesc,

    /// <summary>Sort by date added (ascending)</summary>
    Date,

    /// <summary>Sort by date added (descending)</summary>
    DateDesc
}

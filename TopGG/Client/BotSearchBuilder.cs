using TopGG.Enums;
using TopGG.Models;

namespace TopGG.Client;

/// <summary>
/// Fluent builder for bot search queries.
/// </summary>
public class BotSearchBuilder
{
    private readonly ITopGGClient _client;
    private int? _limit;
    private int? _offset;
    private BotSortField? _sort;
    private List<string>? _fields;

    internal BotSearchBuilder(ITopGGClient client)
    {
        _client = client;
    }

    /// <summary>
    /// Sets the maximum number of bots to return (max 500).
    /// </summary>
    /// <param name="limit">The limit.</param>
    /// <returns>The builder.</returns>
    public BotSearchBuilder WithLimit(int limit)
    {
        _limit = limit;
        return this;
    }

    /// <summary>
    /// Sets the number of bots to skip.
    /// </summary>
    /// <param name="offset">The offset.</param>
    /// <returns>The builder.</returns>
    public BotSearchBuilder WithOffset(int offset)
    {
        _offset = offset;
        return this;
    }

    /// <summary>
    /// Sets the field to sort by.
    /// </summary>
    /// <param name="field">The sort field (use Desc variants for descending order).</param>
    /// <returns>The builder.</returns>
    public BotSearchBuilder SortBy(BotSortField field)
    {
        _sort = field;
        return this;
    }

    /// <summary>
    /// Sets the fields to include in the response.
    /// </summary>
    /// <param name="fields">The fields to include.</param>
    /// <returns>The builder.</returns>
    public BotSearchBuilder WithFields(params string[] fields)
    {
        _fields = fields.ToList();
        return this;
    }

    /// <summary>
    /// Adds a field to include in the response.
    /// </summary>
    /// <param name="field">The field to include.</param>
    /// <returns>The builder.</returns>
    public BotSearchBuilder IncludeField(string field)
    {
        _fields ??= new List<string>();
        _fields.Add(field);
        return this;
    }

    /// <summary>
    /// Executes the search query.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The search results.</returns>
    public async Task<BotSearchResult> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        string? sortValue = null;
        if (_sort.HasValue)
        {
            sortValue = ConvertSortFieldToString(_sort.Value);
        }

        var fieldsValue = _fields != null ? string.Join(",", _fields) : null;

        return await _client.SearchBotsAsync(_limit, _offset, sortValue, fieldsValue, cancellationToken).ConfigureAwait(false);
    }

    private static string ConvertSortFieldToString(BotSortField field)
    {
        return field switch
        {
            BotSortField.Username => "username",
            BotSortField.UsernameDesc => "-username",
            BotSortField.Id => "id",
            BotSortField.IdDesc => "-id",
            BotSortField.ServerCount => "server_count",
            BotSortField.ServerCountDesc => "-server_count",
            BotSortField.Points => "points",
            BotSortField.PointsDesc => "-points",
            BotSortField.MonthlyPoints => "monthlyPoints",
            BotSortField.MonthlyPointsDesc => "-monthlyPoints",
            BotSortField.Date => "date",
            BotSortField.DateDesc => "-date",
            _ => throw new ArgumentOutOfRangeException(nameof(field))
        };
    }
}

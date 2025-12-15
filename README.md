# TopGG

[![NuGet Version](https://img.shields.io/nuget/v/TopGG.svg)](https://www.nuget.org/packages/TopGG)
[![NuGet Downloads](https://img.shields.io/nuget/dt/TopGG.svg)](https://www.nuget.org/packages/TopGG)
[![CI](https://github.com/ziomek64/TopGG/actions/workflows/ci.yml/badge.svg)](https://github.com/ziomek64/TopGG/actions/workflows/ci.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A .NET client library for the [Top.gg API](https://top.gg) - manage Discord bot stats, votes, and more in your .NET applications.

## Installation

```bash
dotnet add package TopGG
```

## Features

- Full Top.gg API v0 and v1 support
- Fully async/await API
- Type-safe enums for sorting and Discord commands
- Fluent API for bot search queries
- Built-in dependency injection support
- Comprehensive error handling with RFC 7807 ProblemDetails
- Rate limit handling with retry information
- Supports .NET Standard 2.0+ (.NET Framework 4.6.2+, .NET Core 2.0+, .NET 5+)
- Cancellation token support

## Quick Start

```csharp
using TopGG.Client;

// Option 1: Direct instantiation
using var client = new TopGGClient("your-topgg-token", botId: 123456789012345678);

// Option 2: Using the builder pattern
var client = TopGGClientBuilder.Create()
    .WithToken("your-topgg-token")
    .WithBotId(123456789012345678)
    .Build();

// Option 3: From environment variables (recommended for production)
var client = TopGGClientBuilder.Create()
    .WithTokenFromEnvironment()  // Reads TOPGG_TOKEN
    .WithBotIdFromEnvironment()  // Reads TOPGG_BOT_ID
    .Build();

// Search for bots
var result = await client.SearchBotsAsync(limit: 5);
foreach (var bot in result.Results)
{
    Console.WriteLine($"{bot.Username} has {bot.Points} votes!");
}

// Post your bot's guild count
await client.PostBotStatsAsync(guildCount: 1500);

// Check if a user has voted
var voteCheck = await client.CheckUserVoteAsync(987654321012345678);
Console.WriteLine($"User has voted: {voteCheck.HasVoted}");
```

## Usage

### Basic Usage

```csharp
// Direct instantiation
using var client = new TopGGClient("your-topgg-token", botId: 123456789012345678);

// Or using builder
var client = TopGGClientBuilder.Create()
    .WithToken("your-topgg-token")
    .WithBotId(123456789012345678)
    .Build();

// Search for bots
var bots = await client.SearchBotsAsync(limit: 10);
foreach (var bot in bots.Results)
{
    Console.WriteLine($"Bot: {bot.Username}");
    Console.WriteLine($"Servers: {bot.ServerCount}");
    Console.WriteLine($"Votes: {bot.Points}");
}

// Get your bot's stats
var stats = await client.GetBotStatsAsync();
Console.WriteLine($"Server Count: {stats.ServerCount}");

// Post your bot's stats
await client.PostBotStatsAsync(guildCount: 1500);

// Get last 1000 voters
var votes = await client.GetBotVotesAsync();
foreach (var vote in votes.Take(5))
{
    Console.WriteLine($"Voter: {vote.Username} ({vote.Id})");
}
```

### Fluent Search API

```csharp
using TopGG.Enums;

// Search for bots with fluent API
var result = await client.Search()
    .WithLimit(10)
    .SortBy(BotSortField.PointsDesc)  // Sort by votes descending
    .ExecuteAsync();

foreach (var bot in result.Results)
{
    Console.WriteLine($"{bot.Username}: {bot.Points} votes");
}
```

### Vote Status (v1 API)

```csharp
// Get detailed vote status
var voteStatus = await client.GetVoteStatusAsync(987654321012345678);
Console.WriteLine($"Last voted: {voteStatus.CreatedAt}");
Console.WriteLine($"Can vote again: {voteStatus.ExpiresAt}");
Console.WriteLine($"Vote weight: {voteStatus.Weight}");
```

### Update Bot Commands on Top.gg (v1 API)

```csharp
using TopGG.Models.Discord;
using TopGG.Enums.Discord;

var commands = new[]
{
    new ApplicationCommand
    {
        Name = "help",
        Description = "Shows the help menu",
        Type = ApplicationCommandType.ChatInput
    },
    new ApplicationCommand
    {
        Name = "ping",
        Description = "Check bot latency",
        Type = ApplicationCommandType.ChatInput,
        Options = new List<ApplicationCommandOption>
        {
            new()
            {
                Name = "ephemeral",
                Description = "Whether to show the response only to you",
                Type = ApplicationCommandOptionType.Boolean,
                Required = false
            }
        }
    }
};

await client.UpdateBotCommandsAsync(commands);
```

### Dependency Injection (ASP.NET Core, etc.)

```csharp
// In Program.cs or Startup.cs
services.AddTopGGClient("your-topgg-token", botId: 123456789012345678);

// In your service/controller
public class BotService
{
    private readonly ITopGGClient _client;

    public BotService(ITopGGClient client)
    {
        _client = client;
    }

    public async Task PostStatsAsync(int serverCount)
    {
        await _client.PostBotStatsAsync(serverCount);
    }

    public async Task<bool> HasUserVotedAsync(ulong userId)
    {
        var result = await _client.CheckUserVoteAsync(userId);
        return result.HasVoted;
    }
}
```

### Custom HttpClient Configuration

```csharp
services.AddTopGGClient("your-topgg-token", botId: 123456789012345678, httpClient =>
{
    httpClient.Timeout = TimeSpan.FromSeconds(30);
});
```

### Error Handling

```csharp
using TopGG.Exceptions;

try
{
    var stats = await client.GetBotStatsAsync();
    await client.PostBotStatsAsync(guildCount: 100);
}
catch (TopGGNotFoundException ex)
{
    // Resource not found (404)
    Console.WriteLine($"Not found: {ex.Message}");
}
catch (TopGGRateLimitException ex)
{
    // Rate limited (429)
    Console.WriteLine($"Rate limited! Retry after: {ex.RetryAfter}");
}
catch (TopGGBadRequestException ex)
{
    // Client errors (400-level)
    Console.WriteLine($"Bad request ({ex.StatusCode}): {ex.Message}");
}
catch (TopGGServerException ex)
{
    // Server errors (500-level)
    Console.WriteLine($"Server error ({ex.StatusCode}): {ex.Message}");
}
catch (TopGGException ex)
{
    // Network errors, timeouts, etc.
    Console.WriteLine($"Error: {ex.Message}");
}
```

## API Reference

### Bot Endpoints (v0)

| Method | Returns | Description |
|--------|---------|-------------|
| `SearchBotsAsync(limit?, offset?, sort?, fields?, cancellationToken?)` | `BotSearchResult` | Search for bots on Top.gg |
| `GetBotStatsAsync(cancellationToken?)` | `BotStats` | Get **your** bot's statistics (requires bot ID in client) |
| `PostBotStatsAsync(guildCount, cancellationToken?)` | `Task` | Post **your** bot's guild count |
| `GetBotVotesAsync(cancellationToken?)` | `Vote[]` | Get last 1000 voters for **your** bot |
| `CheckUserVoteAsync(userId, cancellationToken?)` | `VoteCheck` | Check if a user voted in last 12 hours |

### Project Endpoints (v1)

| Method | Returns | Description |
|--------|---------|-------------|
| `UpdateBotCommandsAsync(commands, cancellationToken?)` | `Task` | Update slash commands on Top.gg |
| `GetVoteStatusAsync(userId, source?, cancellationToken?)` | `VoteStatus` | Get detailed vote status (timestamps, weight) |

### Fluent Search API

The `Search()` method returns a `BotSearchBuilder` for constructing queries:

| Method | Description |
|--------|-------------|
| `Search()` | Create a search builder |
| `.WithLimit(n)` | Set max results (max 500, default 50) |
| `.WithOffset(n)` | Skip n results (for pagination) |
| `.SortBy(field)` | Sort by field (use `*Desc` variants for descending) |
| `.WithFields(params string[])` | Select specific fields to return |
| `.IncludeField(field)` | Add a single field to the selection |
| `.ExecuteAsync(cancellationToken?)` | Execute the search and return results |

### Sort Fields (BotSortField enum)

Available sort fields for the `SortBy()` method:

- `Username` / `UsernameDesc` - Sort by bot username
- `Id` / `IdDesc` - Sort by bot ID
- `ServerCount` / `ServerCountDesc` - Sort by server count
- `Points` / `PointsDesc` - Sort by total votes
- `MonthlyPoints` / `MonthlyPointsDesc` - Sort by monthly votes
- `Date` / `DateDesc` - Sort by submission date

### Response Models

**Bot** - Returned from search results
```csharp
public class Bot
{
    public ulong Id { get; set; }
    public string Username { get; set; }
    public int Points { get; set; }           // Total votes
    public int MonthlyPoints { get; set; }   // Monthly votes
    public int? ServerCount { get; set; }
    public string? Prefix { get; set; }
    public string? ShortDescription { get; set; }
    public List<string>? Tags { get; set; }
    public string? Website { get; set; }
    public string? Invite { get; set; }
    // ... and more properties
}
```

**BotStats** - Your bot's statistics
```csharp
public class BotStats
{
    public int? ServerCount { get; set; }
}
```

**Vote** - A vote from GetBotVotesAsync()
```csharp
public class Vote
{
    public ulong Id { get; set; }
    public string Username { get; set; }
    public string? Avatar { get; set; }
}
```

**VoteCheck** - Result from CheckUserVoteAsync()
```csharp
public class VoteCheck
{
    public int Voted { get; set; }
    public bool HasVoted { get; }  // Computed property (true if Voted == 1)
}
```

**VoteStatus** - Detailed vote info from GetVoteStatusAsync()
```csharp
public class VoteStatus
{
    public DateTime CreatedAt { get; set; }   // When user last voted
    public DateTime ExpiresAt { get; set; }   // When user can vote again
    public int Weight { get; set; }           // Vote multiplier
}
```

## Configuration

### Builder Pattern

The `TopGGClientBuilder` provides a fluent API for creating clients:

```csharp
var client = TopGGClientBuilder.Create()
    .WithToken("your-token")
    .WithBotId(123456789012345678)
    .Build();
```

### Environment Variables

The builder supports loading configuration from environment variables:

```csharp
// Uses default variable names: TOPGG_TOKEN and TOPGG_BOT_ID
var client = TopGGClientBuilder.Create()
    .WithTokenFromEnvironment()
    .WithBotIdFromEnvironment()
    .Build();

// Or use custom environment variable names
var client = TopGGClientBuilder.Create()
    .WithTokenFromEnvironment("MY_CUSTOM_TOKEN")
    .WithBotIdFromEnvironment("MY_CUSTOM_BOT_ID")
    .Build();
```

**Default environment variables:**

| Variable | Description |
|----------|-------------|
| `TOPGG_TOKEN` | Your Top.gg API token |
| `TOPGG_BOT_ID` | Your bot's Discord ID |

### Safe Builder with TryBuild

```csharp
var builder = TopGGClientBuilder.Create()
    .WithTokenFromEnvironment()
    .WithBotIdFromEnvironment();

if (builder.TryBuild(out var client))
{
    // Client created successfully
    await client.PostBotStatsAsync(100);
}
else
{
    Console.WriteLine("Failed to create client - check your configuration");
}

## License

MIT

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## Credits

This library is a wrapper for the [Top.gg API](https://docs.top.gg).

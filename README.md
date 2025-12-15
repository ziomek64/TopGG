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
- Supports .NET Standard 2.0+ (.NET Framework 4.6.1+, .NET Core 2.0+, .NET 5+)
- Cancellation token support

## Quick Start

```csharp
using TopGG.Client;

// Create a client with your token and bot ID
using var client = new TopGGClient("your-topgg-token", botId: 123456789012345678);

// Get a bot's information
var bot = await client.GetBotAsync(123456789012345678);
Console.WriteLine($"{bot.Username} has {bot.Points} votes!");

// Post your bot's server count
await client.PostBotStatsAsync(serverCount: 1500);

// Check if a user has voted
var voteCheck = await client.CheckUserVoteAsync(987654321012345678);
Console.WriteLine($"User has voted: {voteCheck.HasVoted}");
```

## Usage

### Basic Usage

```csharp
using var client = new TopGGClient("your-topgg-token", botId: 123456789012345678);

// Get bot information
var bot = await client.GetBotAsync(123456789012345678);
Console.WriteLine($"Bot: {bot.Username}");
Console.WriteLine($"Servers: {bot.ServerCount}");
Console.WriteLine($"Votes: {bot.Points}");

// Get bot stats
var stats = await client.GetBotStatsAsync(123456789012345678);
Console.WriteLine($"Server Count: {stats.ServerCount}");

// Post bot stats
await client.PostBotStatsAsync(serverCount: 1500);

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
    var bot = await client.GetBotAsync(123456789012345678);
}
catch (TopGGNotFoundException ex)
{
    // Bot not found (404)
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

| Method | Description |
|--------|-------------|
| `SearchBotsAsync(...)` | Search for bots on Top.gg |
| `GetBotAsync(botId)` | Get a bot by Discord ID |
| `GetBotStatsAsync(botId)` | Get bot statistics |
| `PostBotStatsAsync(serverCount, ...)` | Post your bot's server count |
| `GetBotVotesAsync()` | Get last 1000 voters for your bot |
| `CheckUserVoteAsync(userId)` | Check if a user voted in last 12 hours |

### User Endpoints (v0)

| Method | Description |
|--------|-------------|
| `GetUserAsync(userId)` | Get a user by Discord ID |

### Project Endpoints (v1)

| Method | Description |
|--------|-------------|
| `UpdateBotCommandsAsync(commands)` | Update slash commands on Top.gg |
| `GetVoteStatusAsync(userId)` | Get detailed vote status for a user |

### Fluent Search API

| Method | Description |
|--------|-------------|
| `Search()` | Create a search builder |
| `.WithLimit(n)` | Set max results (max 500) |
| `.WithOffset(n)` | Skip n results |
| `.SortBy(field)` | Sort by field (use `*Desc` for descending) |
| `.WithFields(...)` | Select specific fields |
| `.ExecuteAsync()` | Execute the search |

### Sort Fields

- `Username` / `UsernameDesc`
- `Id` / `IdDesc`
- `ServerCount` / `ServerCountDesc`
- `Points` / `PointsDesc`
- `MonthlyPoints` / `MonthlyPointsDesc`
- `Date` / `DateDesc`

## Configuration

### Environment Variables

For the example projects and CI, you can set:

| Variable | Description |
|----------|-------------|
| `TOPGG_TOKEN` | Your Top.gg API token |
| `TOPGG_BOT_ID` | Your bot's Discord ID |

## License

MIT

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## Credits

This library is a wrapper for the [Top.gg API](https://docs.top.gg).

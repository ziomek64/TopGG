using TopGG.Client;
using TopGG.Enums;
using TopGG.Exceptions;
using TopGG.Models.Discord;
using TopGG.Enums.Discord;

var isCI = Environment.GetEnvironmentVariable("CI") == "true";

Console.WriteLine("Top.gg API Client Example");
Console.WriteLine("=========================\n");

// Use fluent builder to create client from environment variables
var builder = TopGGClientBuilder.Create()
    .WithTokenFromEnvironment()
    .WithBotIdFromEnvironment();

if (!builder.TryBuild(out var client) || !builder.BotId.HasValue)
{
    Console.WriteLine("Environment variables TOPGG_TOKEN and TOPGG_BOT_ID are required.");
    Console.WriteLine("Set them before running this example:");
    Console.WriteLine("  export TOPGG_TOKEN=your-token-here");
    Console.WriteLine("  export TOPGG_BOT_ID=123456789012345678");

    if (isCI)
    {
        Console.WriteLine("\nRunning in CI without tokens - skipping API calls.");
        return;
    }

    Console.WriteLine("\nPress any key to exit...");
    Console.ReadKey();
    return;
}

var botId = builder.BotId!.Value;
using var topggClient = client!;

try
{
    // Example 1: Search for bots
    Console.WriteLine("1. Searching for top 5 bots by points...");
    var searchResult = await topggClient.SearchBotsAsync(limit: 5, sort: "-points");
    foreach (var bot in searchResult.Results)
    {
        Console.WriteLine($"   - {bot.Username} (ID: {bot.Id}) - {bot.Points} points");
    }

    // Example 2: Using fluent API for search
    Console.WriteLine("\n2. Using fluent API to search bots...");
    var fluentResult = await topggClient.Search()
        .WithLimit(3)
        .SortBy(BotSortField.MonthlyPointsDesc)
        .ExecuteAsync();

    foreach (var bot in fluentResult.Results)
    {
        Console.WriteLine($"   - {bot.Username} - {bot.MonthlyPoints} monthly points");
    }

    // Example 3: Get bot stats
    Console.WriteLine("\n3. Getting bot stats...");
    var stats = await topggClient.GetBotStatsAsync();
    Console.WriteLine($"   Server Count: {stats.ServerCount}");

    // Example 4: Post bot stats
    Console.WriteLine("\n4. Posting bot stats...");
    await topggClient.PostBotStatsAsync(guildCount: 500);
    Console.WriteLine("   Stats posted: 500 guilds!");

    // Example 5: Get last 1000 votes
    Console.WriteLine("\n5. Getting recent votes...");
    var votes = await topggClient.GetBotVotesAsync();
    Console.WriteLine($"   Total recent votes: {votes.Length}");
    foreach (var vote in votes.Take(5))
    {
        Console.WriteLine($"   - {vote.Username} ({vote.Id})");
    }

    // Example 6: Update bot commands (v1 API)
    Console.WriteLine("\n6. Updating bot commands...");
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
    await topggClient.UpdateBotCommandsAsync(commands);
    Console.WriteLine("   Commands updated successfully!");
}
catch (TopGGNotFoundException ex)
{
    Console.WriteLine($"Not found: {ex.Message}");
}
catch (TopGGRateLimitException ex)
{
    Console.WriteLine($"Rate limited! Retry after: {ex.RetryAfter}");
}
catch (TopGGBadRequestException ex)
{
    Console.WriteLine($"Bad request ({ex.StatusCode}): {ex.Message}");
}
catch (TopGGServerException ex)
{
    Console.WriteLine($"Server error ({ex.StatusCode}): {ex.Message}");
}
catch (TopGGException ex)
{
    Console.WriteLine($"API error: {ex.Message}");
}

Console.WriteLine("\nDone!");

if (!isCI)
{
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}

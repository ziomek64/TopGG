using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TopGG.Client;
using TopGG.Enums;
using TopGG.Exceptions;
using TopGG.Extensions;

// Read configuration from environment variables
var token = Environment.GetEnvironmentVariable("TOPGG_TOKEN");
var botIdStr = Environment.GetEnvironmentVariable("TOPGG_BOT_ID");
var isCI = Environment.GetEnvironmentVariable("CI") == "true";

Console.WriteLine("Top.gg API Client - Dependency Injection Example");
Console.WriteLine("================================================\n");

if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(botIdStr) || !ulong.TryParse(botIdStr, out var botId))
{
    Console.WriteLine("Environment variables TOPGG_TOKEN and TOPGG_BOT_ID are required.");
    Console.WriteLine("Set them before running this example:");
    Console.WriteLine("  export TOPGG_TOKEN=your-token-here");
    Console.WriteLine("  export TOPGG_BOT_ID=123456789012345678");
    Console.WriteLine("\nAlternatively, use the fluent builder:");
    Console.WriteLine("  TopGGClientBuilder.Create()");
    Console.WriteLine("      .WithTokenFromEnvironment()");
    Console.WriteLine("      .WithBotIdFromEnvironment()");
    Console.WriteLine("      .Build()");

    if (isCI)
    {
        Console.WriteLine("\nRunning in CI without tokens - skipping API calls.");
        return;
    }

    Console.WriteLine("\nPress any key to exit...");
    Console.ReadKey();
    return;
}

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        // Register the Top.gg client with DI
        services.AddTopGGClient(token, botId, httpClient =>
        {
            httpClient.Timeout = TimeSpan.FromSeconds(30);
        });

        // Register our example service
        services.AddTransient<BotStatsService>();
    })
    .Build();

// Get the service and run examples
var statsService = host.Services.GetRequiredService<BotStatsService>();
await statsService.RunExamplesAsync();

Console.WriteLine("\nDone!");

if (!isCI)
{
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}

/// <summary>
/// Example service that uses the Top.gg client via dependency injection.
/// </summary>
public class BotStatsService
{
    private readonly ITopGGClient _topGGClient;

    public BotStatsService(ITopGGClient topGGClient)
    {
        _topGGClient = topGGClient;
    }

    public async Task RunExamplesAsync()
    {
        try
        {
            // Example 1: Search for bots using fluent API
            Console.WriteLine("1. Searching for top bots...");
            var searchResult = await _topGGClient.Search()
                .WithLimit(5)
                .SortBy(BotSortField.PointsDesc)
                .ExecuteAsync();

            foreach (var bot in searchResult.Results)
            {
                Console.WriteLine($"   - {bot.Username}: {bot.Points} votes");
            }

            // Example 2: Post stats
            Console.WriteLine("\n2. Posting bot stats...");
            await _topGGClient.PostBotStatsAsync(guildCount: 150);
            Console.WriteLine("   Stats posted: 150 guilds!");

            // Example 3: Check votes
            Console.WriteLine("\n3. Getting recent voters...");
            var votes = await _topGGClient.GetBotVotesAsync();
            Console.WriteLine($"   Recent voters: {votes.Length}");
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
    }
}

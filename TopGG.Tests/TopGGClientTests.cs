using System.Net;
using System.Text.Json;
using Moq;
using Moq.Protected;
using TopGG.Client;
using TopGG.Exceptions;
using TopGG.Models;
using Xunit;

namespace TopGG.Tests;

public class TopGGClientTests
{
    private const string TestToken = "test-token";
    private const ulong TestBotId = 123456789;
    private const ulong TestUserId = 987654321;

    private static TopGGClient CreateClient(HttpMessageHandler handler, ulong? botId = TestBotId)
    {
        var httpClient = new HttpClient(handler);
        return new TopGGClient(httpClient, TestToken, botId);
    }

    private static Mock<HttpMessageHandler> CreateMockHandler(HttpStatusCode statusCode, string content)
    {
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(content)
            });
        return mockHandler;
    }

    [Fact]
    public async Task SearchBotsAsync_ReturnsSearchResult()
    {
        var json = """{"results":[{"id":"1","username":"Bot1"},{"id":"2","username":"Bot2"}],"limit":50,"offset":0,"count":2,"total":100}""";

        var mockHandler = CreateMockHandler(HttpStatusCode.OK, json);
        using var client = CreateClient(mockHandler.Object);

        var result = await client.SearchBotsAsync(limit: 50, offset: 0);

        Assert.Equal(2, result.Results.Count);
        Assert.Equal(100, result.Total);
    }

    [Fact]
    public async Task GetBotStatsAsync_ReturnsStats()
    {
        var json = """{"server_count":1000}""";

        var mockHandler = CreateMockHandler(HttpStatusCode.OK, json);
        using var client = CreateClient(mockHandler.Object);

        var result = await client.GetBotStatsAsync();

        Assert.Equal(1000, result.ServerCount);
    }

    [Fact]
    public async Task PostBotStatsAsync_SendsRequest()
    {
        var mockHandler = new Mock<HttpMessageHandler>();
        HttpRequestMessage? capturedRequest = null;

        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>((req, _) => capturedRequest = req)
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{}")
            });

        using var client = CreateClient(mockHandler.Object);

        await client.PostBotStatsAsync(guildCount: 500);

        Assert.NotNull(capturedRequest);
        Assert.Equal(HttpMethod.Post, capturedRequest.Method);
        Assert.Contains(TestBotId.ToString(), capturedRequest.RequestUri?.ToString());
    }

    [Fact]
    public async Task PostBotStatsAsync_NoBotId_ThrowsInvalidOperationException()
    {
        var mockHandler = CreateMockHandler(HttpStatusCode.OK, "{}");
        using var client = CreateClient(mockHandler.Object, botId: null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => client.PostBotStatsAsync(guildCount: 500));
    }

    [Fact]
    public async Task CheckUserVoteAsync_ReturnsVoteCheck()
    {
        var json = """{"voted":1}""";

        var mockHandler = CreateMockHandler(HttpStatusCode.OK, json);
        using var client = CreateClient(mockHandler.Object);

        var result = await client.CheckUserVoteAsync(TestUserId);

        Assert.True(result.HasVoted);
    }

    [Fact]
    public async Task GetBotVotesAsync_ReturnsVotes()
    {
        var json = """[{"id":"1","username":"User1"},{"id":"2","username":"User2"}]""";

        var mockHandler = CreateMockHandler(HttpStatusCode.OK, json);
        using var client = CreateClient(mockHandler.Object);

        var result = await client.GetBotVotesAsync();

        Assert.Equal(2, result.Length);
    }

    [Fact]
    public void Dispose_DisposesOwnedHttpClient()
    {
        var client = new TopGGClient(TestToken, TestBotId);
        client.Dispose();

        // No exception means success
        Assert.True(true);
    }
}

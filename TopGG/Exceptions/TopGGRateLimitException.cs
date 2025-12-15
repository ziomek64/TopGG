using System.Net;

namespace TopGG.Exceptions;

/// <summary>
/// Exception thrown when the Top.gg API rate limit is exceeded (429).
/// </summary>
public class TopGGRateLimitException : TopGGBadRequestException
{
    private const HttpStatusCode TooManyRequests = (HttpStatusCode)429;

    /// <summary>
    /// The time after which the rate limit resets, if available.
    /// </summary>
    public TimeSpan? RetryAfter { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TopGGRateLimitException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="retryAfter">The time after which to retry.</param>
    public TopGGRateLimitException(string message, TimeSpan? retryAfter = null)
        : base(message, TooManyRequests)
    {
        RetryAfter = retryAfter;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TopGGRateLimitException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="retryAfter">The time after which to retry.</param>
    /// <param name="innerException">The inner exception.</param>
    public TopGGRateLimitException(string message, TimeSpan? retryAfter, Exception innerException)
        : base(message, TooManyRequests, innerException)
    {
        RetryAfter = retryAfter;
    }
}

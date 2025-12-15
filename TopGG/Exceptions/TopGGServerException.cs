using System.Net;

namespace TopGG.Exceptions;

/// <summary>
/// Exception thrown when the Top.gg API returns a 5xx server error.
/// </summary>
public class TopGGServerException : TopGGException
{
    /// <summary>
    /// The HTTP status code returned by the API.
    /// </summary>
    public HttpStatusCode StatusCode { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TopGGServerException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="statusCode">The HTTP status code.</param>
    public TopGGServerException(string message, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = statusCode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TopGGServerException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <param name="innerException">The inner exception.</param>
    public TopGGServerException(string message, HttpStatusCode statusCode, Exception innerException)
        : base(message, innerException)
    {
        StatusCode = statusCode;
    }
}

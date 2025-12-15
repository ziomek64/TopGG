using System.Net;

namespace TopGG.Exceptions;

/// <summary>
/// Exception thrown when a requested resource is not found (404).
/// </summary>
public class TopGGNotFoundException : TopGGBadRequestException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TopGGNotFoundException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public TopGGNotFoundException(string message) : base(message, HttpStatusCode.NotFound)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TopGGNotFoundException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public TopGGNotFoundException(string message, Exception innerException)
        : base(message, HttpStatusCode.NotFound, innerException)
    {
    }
}

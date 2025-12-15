namespace TopGG.Exceptions;

/// <summary>
/// Base exception for all Top.gg API errors.
/// </summary>
public class TopGGException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TopGGException"/> class.
    /// </summary>
    public TopGGException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TopGGException"/> class with a message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public TopGGException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TopGGException"/> class with a message and inner exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public TopGGException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

using System.Runtime.Serialization;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProblemDetailsFactory = Microsoft.AspNetCore.Mvc.Infrastructure.ProblemDetailsFactory;

namespace SoftwareMind.Core.Exceptions;

[Serializable]
public abstract class MappedException : Exception
{
    /// <inheritdoc />
    protected MappedException()
    {
    }

    /// <inheritdoc />
    protected MappedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    /// <inheritdoc />
    protected MappedException(string? message) : base(message)
    {
    }

    /// <inheritdoc />
    protected MappedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    /// <summary>Transforms this exception into the <see cref="ProblemDetails" /> instance.</summary>
    /// <param name="context">The current HTTP context.</param>
    /// <param name="problemFactory">The factory responsible for manufacturing <see cref="ProblemDetails" /> instances.</param>
    /// <returns>A new instance of the <see cref="ProblemDetails" /> class created using current exception.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="context" /> is <see langword="null" /> --or--
    ///     <paramref name="problemFactory" /> is <see langword="null" />..
    /// </exception>
    public virtual ProblemDetails ToProblemDetails(HttpContext context, ProblemDetailsFactory problemFactory)
    {
        return new StatusCodeProblemDetails(StatusCodes.Status500InternalServerError);
    }
}
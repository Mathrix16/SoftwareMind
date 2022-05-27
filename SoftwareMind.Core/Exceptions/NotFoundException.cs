using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace SoftwareMind.Core.Exceptions;

[Serializable]
public sealed class NotFoundException : MappedException
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="NotFoundException" /> class with the specified custom error
    ///     message.
    /// </summary>
    /// <param name="message">The custom message that describes the error.</param>
    public NotFoundException(string? message) : base(message)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="NotFoundException" /> class with the specified resource, which
    ///     presumably is not found, and with the resource's identifier. The exception will contain a generic message which is
    ///     suitable for most scenarios.
    /// </summary>
    /// <param name="resourceName">The human-readable name of the missing resource.</param>
    /// <param name="resourceId">The identifier of the missing resource.</param>
    public NotFoundException(string resourceName, object resourceId) : base(
        $"The requested resource, {resourceName}, could not be found under the specified identifier, {resourceId}.")
    {
    }

    /// <summary>Initializes a new instance of the <see cref="NotFoundException" /> class.</summary>
    public NotFoundException()
    {
    }

    /// <inheritdoc />
    public NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    /// <summary>
    ///     A value indicating whether the missing resource is removed permanently and whether this exception
    ///     should generate 410 Gone HTTP response instead of the standard 404 Not Found one. The default value is
    ///     <see langword="false" />.
    /// </summary>
    public bool IsPermanent { get; set; }

    /// <inheritdoc />
    public override ProblemDetails ToProblemDetails(HttpContext context,
        ProblemDetailsFactory problemDetailsFactory)
    {
        return problemDetailsFactory.CreateProblemDetails(
            context,
            IsPermanent ? StatusCodes.Status410Gone : StatusCodes.Status404NotFound,
            detail: Message
        );
    }
}
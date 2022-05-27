using System.Net;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace SoftwareMind.Core.Exceptions;

[Serializable]
public sealed class InternalServerErrorException : MappedException
{
    /// <inheritdoc />
    public InternalServerErrorException()
    {
    }

    /// <inheritdoc />
    public InternalServerErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    /// <inheritdoc />
    public InternalServerErrorException(string? message) : base(message)
    {
    }

    /// <inheritdoc />
    public InternalServerErrorException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    /// <summary>The HTTP status code associated with this exception. The default value is 400 Bad Request.</summary>
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.InternalServerError;

    /// <inheritdoc />
    public override ProblemDetails ToProblemDetails(HttpContext context,
        ProblemDetailsFactory problemDetailsFactory)
    {
        return problemDetailsFactory.CreateProblemDetails(
            context,
            (int) StatusCode,
            detail: Message
        );
    }
}
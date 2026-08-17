using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ToDos.Backend.API.Utils;

namespace ToDos.Backend.API.Middlewares;

public sealed partial class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{

    [LoggerMessage(
        EventId = 1001,
        Level = LogLevel.Error,
        Message = "An unhandled exception occurred.")]
    private static partial void LogUnhandledException(
        ILogger logger,
        Exception ex);

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        ProblemDetails problemDetails;
        if (exception is ValidationException validationException)
        {
            problemDetails = ProblemDetailsMapper.FromFluentValidation(validationException);
        }
        else
        {
            LogUnhandledException(logger, exception);
            problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server Error",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                Detail = "An unexpected error occurred on the server."
            };
        }

        httpContext.Response.StatusCode = problemDetails.Status!.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}

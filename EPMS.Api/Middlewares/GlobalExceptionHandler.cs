using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;
using FluentValidation;
using FluentValidation.Results; 

namespace EPMS.Api.Middlewares
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment env) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

            var (statusCode, title, detail) = MapException(exception);

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = httpContext.Request.Path
            };

            if (exception is FluentValidation.ValidationException valEx)
            {
                problemDetails.Extensions["errors"] = valEx.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => char.ToLowerInvariant(g.Key[0]) + g.Key.Substring(1),
                        g => g.Select(x => x.ErrorMessage).ToArray()
                    );
            }

            if (env.IsDevelopment())
            {
                problemDetails.Extensions["stackTrace"] = exception.StackTrace;
            }

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }

        private (int StatusCode, string Title, string Detail) MapException(Exception ex) => ex switch
        {
            FluentValidation.ValidationException => ((int)HttpStatusCode.UnprocessableEntity, "Validation Error", "One or more validation failures have occurred."),
            InvalidOperationException => ((int)HttpStatusCode.UnprocessableEntity, "Business Rule Violation", ex.Message),
            ArgumentException => ((int)HttpStatusCode.UnprocessableEntity, "Invalid Argument", ex.Message),

            DbUpdateConcurrencyException => ((int)HttpStatusCode.Conflict, "Concurrency Error", "The data has been modified by another user. Please refresh and try again."),

            KeyNotFoundException => ((int)HttpStatusCode.NotFound, "Not Found", ex.Message),
            UnauthorizedAccessException => ((int)HttpStatusCode.Unauthorized, "Unauthorized", "You do not have permission to access this resource."),

            DbUpdateException => ((int)HttpStatusCode.BadRequest, "Database Error", env.IsDevelopment() ? ex.InnerException?.Message ?? ex.Message : "A database error occurred."),

            _ => ((int)HttpStatusCode.InternalServerError, "Server Error", "An unexpected error occurred on the server.")
        };
    }
}

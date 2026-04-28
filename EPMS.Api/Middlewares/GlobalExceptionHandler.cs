using EPMS.Shared.Constants.EPMS.Shared.Constants;
using FluentValidation;
using FluentValidation.Results; 
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

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
                        g => string.IsNullOrEmpty(g.Key) ? "general" : JsonNamingPolicy.CamelCase.ConvertName(g.Key),
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
            FluentValidation.ValidationException => (
                (int)HttpStatusCode.UnprocessableEntity,
                ErrorMessages.Titles.ValidationError,
                ErrorMessages.Descriptions.ValidationFailure
            ),

            InvalidOperationException => (
                (int)HttpStatusCode.UnprocessableEntity,
                ErrorMessages.Titles.BusinessRuleViolation,
                ex.Message
            ),

            ArgumentException => (
                (int)HttpStatusCode.UnprocessableEntity,
                ErrorMessages.Titles.InvalidArgument,
                ex.Message
            ),

            DbUpdateConcurrencyException => (
                (int)HttpStatusCode.Conflict,
                ErrorMessages.Titles.ConcurrencyError,
                ErrorMessages.Descriptions.ConcurrencyConflict
            ),

            KeyNotFoundException => (
                (int)HttpStatusCode.NotFound,
                ErrorMessages.Titles.NotFound,
                ex.Message
            ),

            UnauthorizedAccessException => (
                (int)HttpStatusCode.Unauthorized,
                ErrorMessages.Titles.Unauthorized,
                ex.Message
            ),

            DbUpdateException => (
                (int)HttpStatusCode.BadRequest,
                ErrorMessages.Titles.DatabaseError,
                env.IsDevelopment() ? ex.InnerException?.Message ?? ex.Message : ErrorMessages.Descriptions.DatabaseErrorGeneric
            ),

            _ => (
                (int)HttpStatusCode.InternalServerError,
                ErrorMessages.Titles.ServerError,
                ErrorMessages.Descriptions.InternalServerError
            )
        };
    }
}

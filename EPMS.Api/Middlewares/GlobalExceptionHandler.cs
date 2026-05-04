using EPMS.Shared.Constants.EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.Enums;
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

            var (statusCode, message, errorType) = MapException(exception);

            Dictionary<string, string[]>? validationErrors = null;

            if (exception is FluentValidation.ValidationException valEx)
            {
                validationErrors = valEx.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => string.IsNullOrEmpty(g.Key) ? "general" : JsonNamingPolicy.CamelCase.ConvertName(g.Key),
                        g => g.Select(x => x.ErrorMessage).ToArray()
                    );
            }

            var response = SuccessResponse.Fail(message, errorType, validationErrors);

            if (env.IsDevelopment() && errorType == ErrorType.ServerError)
            {
                response = response with { Message = $"{message} - {exception.Message}" };
            }

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

            return true;
        }

        private (int StatusCode, string Message, ErrorType ErrorType) MapException(Exception ex) => ex switch
        {
            FluentValidation.ValidationException => (
                (int)HttpStatusCode.BadRequest,
                "Validation failed. Please check the inputted data.",
                ErrorType.Validation
            ),

            UnauthorizedAccessException => (
                (int)HttpStatusCode.Unauthorized,
                "You are not authorized to perform this action.",
                ErrorType.Unauthorized
            ),

            KeyNotFoundException => (
                (int)HttpStatusCode.NotFound,
                "The requested resource was not found.",
                ErrorType.NotFound
            ),

            InvalidOperationException => (
                (int)HttpStatusCode.Conflict,
                ex.Message,
                ErrorType.Conflict
            ),

            _ => (
                (int)HttpStatusCode.InternalServerError,
                "An unexpected internal server error occurred.",
                ErrorType.ServerError
            )
        };
    }
}
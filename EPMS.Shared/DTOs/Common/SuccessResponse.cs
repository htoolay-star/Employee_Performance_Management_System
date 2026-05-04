using EPMS.Shared.Enums;

namespace EPMS.Shared.DTOs.Common
{
    public record SuccessResponse<T> : SuccessResponse
    {
        public T? Data { get; init; }

        public static SuccessResponse<T> Ok(T data, string message ) =>
            new() { Success = true, Message = message, Data = data };

        public new static SuccessResponse<T> Fail(string message, ErrorType errorType = ErrorType.None) =>
            new() { Success = false, Message = message, Error = errorType };
    }

    /// <summary>
    /// Standard response for operations that don't return data but need success status and message.
    /// Can be used across all services (Auth, App, Performance, etc.)
    /// </summary>
    public record SuccessResponse
    {
        public bool Success { get; init; } = true;
        public string Message { get; init; } = string.Empty;
        public ErrorType Error { get; init; } = ErrorType.None;

        public Dictionary<string, string[]>? ValidationErrors { get; init; }

        // Factory methods for common responses
        public static SuccessResponse Ok(string message) =>
            new() { Success = true, Message = message };

        public static SuccessResponse Fail(string message, ErrorType errorType = ErrorType.None, Dictionary<string, string[]>? validationErrors = null) =>
            new() { Success = false, Message = message, Error = errorType, ValidationErrors = validationErrors };
    }
}

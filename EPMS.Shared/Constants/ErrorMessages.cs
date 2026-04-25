using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.Constants
{
    namespace EPMS.Shared.Constants
    {
        public static class ErrorMessages
        {
            public static class Titles
            {
                public const string ValidationError = "Validation Error";
                public const string BusinessRuleViolation = "Business Rule Violation";
                public const string InvalidArgument = "Invalid Argument";
                public const string ConcurrencyError = "Concurrency Error";
                public const string NotFound = "Not Found";
                public const string Unauthorized = "Unauthorized";
                public const string DatabaseError = "Database Error";
                public const string ServerError = "Server Error";
            }

            public static class Descriptions
            {
                public const string ValidationFailure = "One or more validation failures have occurred.";
                public const string ConcurrencyConflict = "The data has been modified by another user. Please refresh and try again.";
                public const string UnauthorizedAccess = "You do not have permission to access this resource.";
                public const string InternalServerError = "An unexpected error occurred on the server.";
                public const string DatabaseErrorGeneric = "A database error occurred.";
            }
        }
    }
}

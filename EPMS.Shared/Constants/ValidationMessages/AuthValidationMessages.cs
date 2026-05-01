using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.Constants.ValidationMessages
{
    public static class AuthValidationMessages
    {
        public static class Email
        {
            public const string Required = "Email address is required.";
            public const string Invalid = "Invalid email format.";
            public const string MaxLength = "Email cannot exceed 256 characters.";
        }

        public static class Password
        {
            public const string Required = "Password is required.";
            public const string MinimumLength = "Password must be at least 8 characters long.";
            public const string NoSpaces = "Password cannot contain spaces.";
            public const string RequiresUppercase = "Password must contain at least one uppercase letter.";
            public const string RequiresLowercase = "Password must contain at least one lowercase letter.";
            public const string RequiresDigit = "Password must contain at least one number.";
            public const string Mismatch = "Passwords do not match.";
            public const string ConfirmRequired = "Confirm password is required.";
        }

        public static class Users
        {
            public const string RoleInvalid = "Please select a valid user role.";
            public const string TargetUserRequired = "Target user identification is required.";
            public const string SystemAdminNotAllowed = "SystemAdmin role cannot be assigned through this endpoint.";
        }

        public static class Tokens
        {
            public const string RefreshTokenRequired = "Refresh token is required to generate a new session.";
        }
    }
}

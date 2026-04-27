namespace EPMS.Shared.Constants.ValidationMessages
{
    public static class PermissionValidationMessages
    {
        public static class Code
        {
            public const string Required = "Permission code is required.";
            public const string MaxLength = "Permission code cannot exceed 50 characters.";
        }

        public static class Name
        {
            public const string Required = "Permission name is required.";
            public const string MaxLength = "Permission name cannot exceed 100 characters.";
        }
    }
}

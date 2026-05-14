namespace EPMS.Shared.Validators.ValidationMessages;

public static class SharedValidationMessages
{
    public static class Category
    {
        public const string CodeRequired = "Category code is required.";
        public const string CodeMaxLength = "Category code cannot exceed 50 characters.";
        public const string NameRequired = "Category name is required.";
        public const string NameMaxLength = "Category name cannot exceed 100 characters.";
        public const string DescriptionMaxLength = "Category description cannot exceed 250 characters.";
        public const string ParentIdInvalid = "Parent category ID must be greater than 0 when provided.";
    }

    public static class DocumentAttachment
    {
        public static class EntityType
        {
            public const string Required = "Entity type is required.";
            public const string MaxLength = "Entity type cannot exceed 50 characters.";
        }

        public static class EntityId
        {
            public const string Invalid = "Entity ID must be greater than 0.";
        }

        public static class FileName
        {
            public const string Required = "File name is required.";
            public const string MaxLength = "File name cannot exceed 255 characters.";
        }

        public static class FilePath
        {
            public const string Required = "File path is required.";
            public const string MaxLength = "File path cannot exceed 500 characters.";
        }

        public static class FileSize
        {
            public const string Invalid = "File size must be greater than 0.";
        }

        public static class MimeType
        {
            public const string Required = "MIME type is required.";
            public const string MaxLength = "MIME type cannot exceed 100 characters.";
        }

        public static class UploadedById
        {
            public const string Invalid = "Uploaded by user ID must be greater than 0.";
        }

        public static class Description
        {
            public const string MaxLength = "Description cannot exceed 500 characters.";
        }

        public static class Category
        {
            public const string MaxLength = "Category cannot exceed 50 characters.";
        }
    }
}

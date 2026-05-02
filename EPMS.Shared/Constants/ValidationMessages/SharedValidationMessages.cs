namespace EPMS.Shared.Constants.ValidationMessages;

public static class SharedValidationMessages
{
    public static class Category
    {
        public const string ModuleRequired = "Category module is required.";
        public const string ModuleMaxLength = "Category module cannot exceed 50 characters.";
        public const string CodeRequired = "Category code is required.";
        public const string CodeMaxLength = "Category code cannot exceed 50 characters.";
        public const string NameRequired = "Category name is required.";
        public const string NameMaxLength = "Category name cannot exceed 100 characters.";
        public const string DescriptionMaxLength = "Category description cannot exceed 250 characters.";
        public const string ParentIdInvalid = "Parent category ID must be greater than 0 when provided.";
    }

    public static class Tag
    {
        public const string NameRequired = "Tag name is required.";
        public const string NameMaxLength = "Tag name cannot exceed 50 characters.";
        public const string ModuleMaxLength = "Tag module cannot exceed 50 characters.";
    }
}

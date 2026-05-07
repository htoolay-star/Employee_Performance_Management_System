namespace EPMS.Shared.Validators.ValidationMessages;

public static class AppValidationMessages
{
    public static class Notification
    {
        public const string ToUserIdInvalid = "Valid user ID is required.";
        public const string TitleRequired = "Notification title is required.";
        public const string TitleMaxLength = "Title cannot exceed 200 characters.";
        public const string MessageRequired = "Notification message is required.";
        public const string MessageMaxLength = "Message cannot exceed 1000 characters.";
        public const string TypeRequired = "Notification type is required.";
        public const string TypeMaxLength = "Type cannot exceed 50 characters.";
        public const string UrlMaxLength = "URL cannot exceed 500 characters.";
        public const string UrlInvalid = "Please provide a valid URL.";
    }
}
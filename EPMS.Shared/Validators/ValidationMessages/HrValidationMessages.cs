namespace EPMS.Shared.Validators.ValidationMessages
{
    public static class HrValidationMessages
    {
        public static class Department
        {
            public const string CodeRequired = "Department code is required.";
            public const string CodeMaxLength = "Department code cannot exceed 20 characters.";
            public const string NameRequired = "Department name is required.";
            public const string NameMaxLength = "Department name cannot exceed 100 characters.";
        }

        public static class Team
        {
            public const string NameRequired = "Team name is required.";
            public const string NameMaxLength = "Team name cannot exceed 100 characters.";
            public const string DepartmentIdRequired = "Department is required.";
            public const string DepartmentIdInvalid = "Please select a valid department.";
        }

        public static class Position
        {
            public const string TitleRequired = "Position title is required.";
            public const string TitleMaxLength = "Position title cannot exceed 100 characters.";
            public const string LevelIdRequired = "Level is required.";
            public const string LevelIdInvalid = "Please select a valid level.";
        }

        public static class Level
        {
            public const string CodeRequired = "Level code is required.";
            public const string CodeMaxLength = "Level code cannot exceed 10 characters.";
            public const string NameRequired = "Level name is required.";
            public const string NameMaxLength = "Level name cannot exceed 100 characters.";
            public const string DescriptionMaxLength = "Level description cannot exceed 250 characters.";
        }
    }
}

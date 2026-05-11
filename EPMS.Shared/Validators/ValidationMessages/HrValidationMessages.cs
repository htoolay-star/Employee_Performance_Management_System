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
            public const string DescriptionMaxLength = "Department description cannot exceed 500 characters.";
            public const string DeptHeadIdInvalid = "Please select a valid department head.";
        }

        public static class Team
        {
            public const string CodeRequired = "Team code is required.";
            public const string CodeMaxLength = "Team code cannot exceed 20 characters.";
            public const string NameRequired = "Team name is required.";
            public const string NameMaxLength = "Team name cannot exceed 100 characters.";
            public const string DescriptionMaxLength = "Team description cannot exceed 500 characters.";
            public const string DepartmentIdRequired = "Department is required.";
            public const string DepartmentIdInvalid = "Please select a valid department.";
            public const string LeadTeamIdInvalid = "Please select a valid team lead.";
        }

        public static class Position
        {
            public const string CodeRequired = "Position code is required.";
            public const string CodeMaxLength = "Position code cannot exceed 20 characters.";
            public const string NameRequired = "Position name is required.";
            public const string NameMaxLength = "Position name cannot exceed 100 characters.";
            public const string LevelIdRequired = "Level is required.";
            public const string LevelIdInvalid = "Please select a valid level.";
            public const string DescriptionMaxLength = "Position description cannot exceed 500 characters.";
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

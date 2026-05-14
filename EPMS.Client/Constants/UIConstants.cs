namespace EPMS.Client.Constants
{
    public static class UIConstants
    {
        public static class Common
        {
            public const string ColumnNo = "No.";
            public const string ColumnName = "Name";
            public const string Description = "Description";
            public const string Cancel = "Cancel";
            public const string Create = "Create";
            public const string Update = "Update";
            public const string Delete = "Delete";
            public const string Active = "Active";
            public const string Inactive = "Inactive";
            public const string Status = "Status";
            public const string Actions = "Actions";
            public const string DuplicateEntry = "Duplicate entry error.";
            public const string ValidationFailed = "Validation failed. Please check your inputs.";
            public const string UpdateFailed = "Failed to update the record.";
            public const string CreateFailed = "Failed to create the record.";
            public const string DeleteFailed = "Failed to delete the record.";
            public const string LoadFailed = "Failed to load data.";
            public const string DeleteConfirmationFormat = "Are you sure you want to delete '{0}'?";
            public const string ErrorFormat = "Error: {0}";
            public const string ErrorOccurred = "An error occurred. Please try again.";
            public const string NoRecords = "No data found.";
            public const string Loading = "Loading...";
        }

        public static class Position
        {
            public const string PageTitle = "Position Management";
            public const string SearchPlaceholder = "Search by name...";
            public const string AddButton = "Add Position";
            public const string CreateDialogTitle = "Create Position";
            public const string EditDialogTitle = "Edit Position";
            public const string DeleteDialogTitle = "Delete Position";
            public const string CodeLabel = "Position Code";
            public const string TitleLabel = "Position Name";
            public const string LevelLabel = "Level";

            public const string ColumnCode = "Position Code";
            public const string ColumnName = "Position Name";
            public const string ColumnLevel = "Level";

            public const string SelectLevelPlaceholder = "-- Select a Level --";
            public const string NoDataFound = "Try adjusting your search or add a new position.";
        }

        public static class Level
        {
            public const string PageTitle = "Level Management";
            public const string SearchPlaceholder = "Search by name...";
            public const string AddButton = "Add Level";
            public const string CreateDialogTitle = "Create Level";
            public const string EditDialogTitle = "Edit Level";
            public const string DeleteDialogTitle = "Delete Level";
            public const string CodeLabel = "Level Code";
            public const string NameLabel = "Level Name";
            public const string ColumnCode = "Level Code";
            public const string ColumnName = "Level Name";

            public const string NoDataFound = "Try adjusting your search or add a new level.";
        }

        public static class Team
        {
            public const string PageTitle = "Team Management";
            public const string SearchPlaceholder = "Search by team name...";
            public const string AddButton = "Add Team";
            public const string CreateDialogTitle = "Create Team";
            public const string EditDialogTitle = "Edit Team";
            public const string DeleteDialogTitle = "Delete Team";
            public const string CodeLabel = "Team Code";
            public const string NameLabel = "Team Name";
            public const string DepartmentLabel = "Department";
            public const string ColumnCode = "Team Code";
            public const string ColumnName = "Team Name";
            public const string ColumnDepartment = "Department";
            public const string SelectDepartmentPlaceholder = "-- Select a Department --";
            public const string LeadColumn = "PM";
            public const string NoDataFound = "Try adjusting your search or add a new team.";
        }

        public static class Category
        {
            public const string PageTitle = "Category Management";
            public const string SearchPlaceholder = "Search by code or name...";
            public const string AddButton = "Add Category";
            public const string CreateDialogTitle = "Create Category";
            public const string EditDialogTitle = "Edit Category";
            public const string DeleteDialogTitle = "Delete Category";
            public const string CodeLabel = "Category Code";
            public const string NameLabel = "Category Name";
            public const string ColumnCode = "Category Code";
            public const string ColumnName = "Category Name";
            public const string SelectParent = "Parent Category";
            public const string NoneRoot = "None (Root)";
            public const string NoDataFound = "Try adjusting your search or add a new category.";
        }

        public static class Department
        {
            public const string PageTitle = "Department Management";
            public const string SearchPlaceholder = "Search by code or name...";
            public const string AddButton = "Add Department";
            public const string CreateDialogTitle = "Create Department";
            public const string EditDialogTitle = "Edit Department";
            public const string DeleteDialogTitle = "Delete Department";
            public const string CodeLabel = "Department Code";
            public const string NameLabel = "Department Name";
            public const string ColumnCode = "Department Code";
            public const string ColumnName = "Department Name";
            public const string HeadColumn = "Head";
            public const string NoDataFound = "Try adjusting your search or add a new department.";
        }

        public static class AppraisalCycle
        {
            public const string PageTitle = "Appraisal Cycles";
            public const string SearchPlaceholder = "Search by name or year...";
            public const string AddButton = "Add Cycle";
            public const string CreateDialogTitle = "Create Appraisal Cycle";
            public const string EditDialogTitle = "Edit Appraisal Cycle";
            public const string DeleteDialogTitle = "Delete Appraisal Cycle";

            public const string NameLabel = "Cycle Name";
            public const string AppraisalTypeLabel = "Appraisal Type";
            public const string CalendarTypeLabel = "Calendar Type";
            public const string YearLabelLabel = "Year Label";
            public const string EvalPeriod = "Evaluation Period";
            public const string EvalStart = "Evaluation Start";
            public const string EvalEnd = "Evaluation End";
            public const string WindowStart = "Window Start";
            public const string WindowEnd = "Window End";
            public const string SelfReview = "Self Review";
            public const string ManagerReview = "Manager Review";
            public const string PeerReview = "Peer Review";

            public const string SelectAppraisalType = "-- Select Type --";
            public const string SelectCalendarType = "-- Select Calendar --";

            public const string ColumnName = "Cycle Name";
            public const string ColumnType = "Type";
            public const string ColumnCalendar = "Calendar";
            public const string ColumnYear = "Year";
            public const string ColumnEvalPeriod = "Evaluation Period";
            public const string ColumnWindowPeriod = "Window Period";
            public const string Lock = "Lock";
            public const string Unlock = "Unlock";
            public const string Deactivate = "Deactivate";
            public const string Reactivate = "Reactivate";
            public const string StatusChipActive = "Active";
            public const string StatusChipInactive = "Inactive";
            public const string StatusChipLocked = "Locked";

            public const string NoDataFound = "No appraisal cycles found. Add one to get started.";
        }

        public static class Employee
        {
            public const string PageTitle = "Employee Directory";
            public const string SearchPlaceholder = "Search by name or staff no...";
            public const string AddButton = "Add Employee";
            public const string ColumnStaffNo = "Staff No.";
            public const string ColumnName = "Staff Name";
            public const string ColumnDepartment = "Department";
            public const string ColumnPosition = "Position";
            public const string ColumnStatus = "Status";
            public const string ColumnActions = "Actions";
            public const string NoDataFound = "Try adjusting your search or add a new employee.";
        }
    }
}

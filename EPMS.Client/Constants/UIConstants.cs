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
            public const string ViewKpiDialogTitle = "KPI Assignments";
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
            public const string ViewKpiDialogTitle = "KPI Assignments";
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
            public const string ViewKpiDialogTitle = "KPI Assignments";
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
            public const string ThreeSixtyReview = "360° Review";
            public const string AppraisalReview = "Appraisal Review";
            public const string KpiReview = "KPI Review";

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

        public static class QuestionRatingScale
        {
            public const string PageTitle = "Question Rating Scale Management";
            public const string SearchPlaceholder = "Search by name...";
            public const string AddButton = "Add Rating Scale";
            public const string CreateDialogTitle = "Create Rating Scale";
            public const string EditDialogTitle = "Edit Rating Scale";
            public const string DeleteDialogTitle = "Delete Rating Scale";

            public const string NameLabel = "Name";
            public const string MinScoreLabel = "Min Score";
            public const string MaxScoreLabel = "Max Score";

            public const string ColumnMinScore = "Score Range";
            public const string ColumnMaxScore = "Levels";

            public const string NoDataFound = "Try adjusting your search or add a new rating scale.";
        }

        public static class EntityKPI
        {
            public const string PageTitle = "KPI Assignments";
            public const string SearchPlaceholder = "Search by KPI name...";
            public const string AddButton = "Add KPI Assignment";
            public const string CreateDialogTitle = "Assign KPI";
            public const string EditDialogTitle = "Edit KPI Assignment";
            public const string DeleteDialogTitle = "Delete KPI Assignment";

            public const string ColumnEntityType = "Type";
            public const string ColumnEntity = "Assigned To";
            public const string ColumnKPI = "KPI";
            public const string ColumnPriority = "Priority";
            public const string ColumnWeightage = "Weight";
            public const string ColumnTarget = "Target";
            public const string ColumnUnit = "Unit";

            public const string NoDataFound = "Select a type and assigned to see KPI assignments.";
        }

        public static class RecycleBin
        {
            public const string PageTitle = "Recycle Bin";
            public const string SearchPlaceholder = "Search by name...";
            public const string ColumnEntityType = "Type";
            public const string ColumnName = "Name";
            public const string ColumnDeletedAt = "Deleted At";
            public const string ColumnDaysLeft = "Days Left";

            public const string NoDataFound = "No deleted items found. The recycle bin is empty.";
        }

        public static class EmployeeKPI
        {
            public const string PageTitle = "Employee KPI Assignments";
            public const string AddButton = "Add KPI";
            public const string CreateDialogTitle = "Assign KPI to Employee";
            public const string EditDialogTitle = "Edit Employee KPI";
            public const string DeleteDialogTitle = "Delete Employee KPI";

            public const string ColumnEmployee = "Employee";
            public const string ColumnKPI = "KPI";
            public const string ColumnPriority = "Priority";
            public const string ColumnWeightage = "Weight";
            public const string ColumnTarget = "Target";
            public const string ColumnUnit = "Unit";

            public const string NoDataFound = "Select an employee and cycle to see KPI assignments.";
        }

        public static class KPIWeightPriority
        {
            public const string PageTitle = "KPI Weight Priority Management";
            public const string SearchPlaceholder = "Search by level name...";
            public const string AddButton = "Add Priority";
            public const string CreateDialogTitle = "Create Priority";
            public const string EditDialogTitle = "Edit Priority";
            public const string DeleteDialogTitle = "Delete Priority";

            public const string LevelNameLabel = "Priority Level";
            public const string MinWeightLabel = "Min Weight (%)";
            public const string MaxWeightLabel = "Max Weight (%)";
            public const string ColorLabel = "Color";

            public const string ColumnLevel = "Priority Level";
            public const string ColumnMinWeight = "Min Weight (%)";
            public const string ColumnMaxWeight = "Max Weight (%)";
            public const string ColumnColor = "Color";

            public const string NoDataFound = "Try adjusting your search or add a new priority.";
        }

        public static class RatingScale
        {
            public const string PageTitle = "Rating Scale Management";
            public const string SearchPlaceholder = "Search by label or rating...";
            public const string AddButton = "Add Rating Scale";
            public const string CreateDialogTitle = "Create Rating Scale";
            public const string EditDialogTitle = "Edit Rating Scale";
            public const string DeleteDialogTitle = "Delete Rating Scale";

            public const string RatingLabel = "Rating";
            public const string LabelLabel = "Label";
            public const string MinScoreLabel = "Min Score";
            public const string MaxScoreLabel = "Max Score";
            public const string PromotionEligibilityLabel = "Promotion Eligibility";

            public const string ColumnRating = "Rating";
            public const string ColumnLabel = "Label";
            public const string ColumnMinScore = "Min Score";
            public const string ColumnMaxScore = "Max Score";
            public const string ColumnPromotionEligibility = "Promotion";

            public const string NoDataFound = "Try adjusting your search or add a new rating scale.";
        }

        public static class FormTemplate
        {
            public const string PageTitle = "Form Template Management";
            public const string SearchPlaceholder = "Search by name...";
            public const string AddButton = "Add Form Template";
            public const string CreateDialogTitle = "Create Form Template";
            public const string EditDialogTitle = "Edit Form Template";
            public const string DeleteDialogTitle = "Delete Form Template";

            public const string NameLabel = "Template Name";
            public const string FormTypeLabel = "Form Type";
            public const string QuestionsPerEvaluationLabel = "Questions Per Evaluation";
            public const string QuestionsPerEvaluationHelper = "Leave empty to use all questions.";

            public const string ColumnName = "Template Name";
            public const string ColumnFormType = "Form Type";
            public const string ColumnRatingScale = "Rating Scale";
            public const string ColumnQuestions = "Questions";
            public const string ColumnQuestionsPerEvaluation = "Per Eval";

            public const string NoDataFound = "Try adjusting your search or add a new form template.";
        }

        public static class PositionFormTemplate
        {
            public const string PageTitle = "Position Form Templates";
            public const string AddButton = "Assign Template";
            public const string CreateDialogTitle = "Assign Template to Position";
            public const string EditDialogTitle = "Edit Template Assignment";
            public const string DeleteDialogTitle = "Delete Assignment";

            public const string FormTemplateLabel = "Form Template";
            public const string MandatoryLabel = "Mandatory";

            public const string ColumnPosition = "Position";
            public const string ColumnTemplate = "Template";
            public const string ColumnMandatory = "Mandatory";

            public const string SelectPosition = "-- Select Position --";
            public const string NoDataFound = "Select a position to see assigned form templates.";
        }

        public static class FormQuestion
        {
            public const string PageTitle = "Template Questions";
            public const string AddButton = "Add Question";
            public const string CreateDialogTitle = "Create Question";
            public const string EditDialogTitle = "Edit Question";
            public const string DeleteDialogTitle = "Delete Question";

            public const string QuestionTextLabel = "Question Text";
            public const string SequenceLabel = "Order";
            public const string CategoryLabel = "Category";
            public const string RatingScaleLabel = "Rating Scale";
            public const string HasYesNoLabel = "Yes/No Response";
            public const string HasCommentLabel = "Allow Comment";

            public const string ColumnNo = "No.";
            public const string ColumnQuestion = "Question";
            public const string ColumnSequence = "Order";
            public const string ColumnCategory = "Category";
            public const string ColumnRatingScale = "Rating Scale";
            public const string ColumnActions = "Actions";

            public const string NoDataFound = "No questions found for this template.";
        }

        public static class KPIMaster
        {
            public const string PageTitle = "KPI Master Management";
            public const string SearchPlaceholder = "Search by code or name...";
            public const string AddButton = "Add KPI Master";
            public const string CreateDialogTitle = "Create KPI Master";
            public const string EditDialogTitle = "Edit KPI Master";
            public const string DeleteDialogTitle = "Delete KPI Master";

            public const string CodeLabel = "KPI Code";
            public const string NameLabel = "KPI Name";
            public const string CategoryLabel = "Category";

            public const string ColumnCode = "KPI Code";
            public const string ColumnName = "KPI Name";
            public const string ColumnCategory = "Category";
            public const string ColumnDescription = "Description";

            public const string NoDataFound = "Try adjusting your search or add a new KPI master.";
            public const string ScoringDirectionLabel = "Scoring Direction";
            public const string ScoringDirectionHigher = "Higher is Better";
            public const string ScoringDirectionLower = "Lower is Better";
        }

        public static class AppraisalFill
        {
            public const string PageTitle = "KPI Evaluation";
            public const string EmployeeLabel = "Employee";
            public const string CycleLabel = "Cycle";
            public const string StatusLabel = "Status";
            public const string ColumnKPI = "KPI";
            public const string ColumnWeight = "Weight";
            public const string ColumnTarget = "Target";
            public const string ColumnDirection = "";
            public const string ColumnActual = "Actual";
            public const string ColumnScore = "Score";
            public const string ColumnWeighted = "Weighted Score";
            public const string ColumnRemarks = "Remarks";
            public const string SubmitButton = "Submit Evaluation";
            public const string SubmitSuccess = "Evaluation submitted successfully.";
            public const string NegativeValueError = "Value cannot be negative";
        }

        public static class MyKpi
        {
            public const string PageTitle = "My KPI";
            public const string NoData = "No KPI records found.";
            public const string FinalizedButton = "Finalize";
            public const string FinalizeSuccess = "Appraisal finalized successfully.";
        }

        public static class Pending
        {
            public const string PageTitle = "Pending Approvals";
            public const string NoData = "No pending approvals.";
        }

        public static class EvaluationPending
        {
            public const string PageTitle = "Pending Evaluations";
            public const string NoData = "No pending evaluations.";
        }

        public static class MySelfAssessments
        {
            public const string PageTitle = "My Self Assessments";
            public const string NoData = "No self assessment forms found.";
        }

        public static class My360Forms
        {
            public const string PageTitle = "My 360 Forms";
            public const string NoData = "No 360 forms found.";
        }

        public static class MyAppraisalForms
        {
            public const string PageTitle = "My Appraisal Forms";
            public const string NoData = "No appraisal forms found.";
        }

        public static class ManagerSelfPending
        {
            public const string PageTitle = "Pending Self Reviews";
            public const string NoData = "No pending self reviews.";
        }

        public static class ManagerSelfReview
        {
            public const string PageTitle = "Self Assessment Review";
            public const string ApproveButton = "Approve Self Assessment";
            public const string Approved = "Self assessment approved successfully.";
        }

        public static class AppraisalView
        {
            public const string PageTitle = "Appraisal View";
            public const string FinalizeButton = "Finalize";
            public const string FinalizeSuccess = "Appraisal finalized successfully.";
        }

        public static class AppraisalList
        {
            public const string PageTitle = "KPI Evaluations";
            public const string SearchPlaceholder = "Search by employee name...";
            public const string ColumnEmployee = "Employee";
            public const string ColumnCycle = "Cycle";
            public const string ColumnStatus = "Status";
            public const string ColumnScore = "Score";
            public const string ColumnLocked = "Locked";
            public const string FillButton = "Fill";
            public const string DeleteConfirmFormat = "Are you sure you want to delete appraisal for '{0}'?";
            public const string DeleteSuccess = "Appraisal deleted successfully.";
            public const string NoDataFound = "No appraisals found.";
        }

        public static class EvaluationForm
        {
            public const string PageTitle = "Evaluation Form";
            public const string EmployeeLabel = "Employee";
            public const string CycleLabel = "Cycle";
            public const string RoleLabel = "Role";
            public const string StatusLabel = "Status";
            public const string ColumnQuestion = "Question";
            public const string ColumnRating = "Rating";
            public const string ColumnComment = "Comment";
            public const string CommentPlaceholder = "Enter comment...";
            public const string SubmitButton = "Submit Evaluation";
            public const string SubmitSuccess = "Evaluation submitted successfully.";
            public const string AlreadySubmitted = "You have already submitted your evaluation.";
            public const string NoFormsAvailable = "No evaluation forms are available for you.";
            public const string RatingRequired = "Please provide a rating for all questions before submitting.";
            public const string YesNoRequired = "Please answer Yes/No for all applicable questions before submitting.";
        }

        public static class DefaultPassword
        {
            public const string PageTitle = "Default Password Settings";
            public const string NewPasswordLabel = "New Default Password";
            public const string ConfirmPasswordLabel = "Confirm Password";
            public const string SaveButton = "Update Password";
            public const string SuccessMessage = "Default password updated successfully.";
        }

        public static class ContinuousFeedback
        {
            public const string PageTitle = "Continuous Feedback";
            public const string SearchPlaceholder = "Search by employee or content...";
            public const string AddButton = "Add Feedback";
            public const string CreateDialogTitle = "Give Feedback";
            public const string EditDialogTitle = "Edit Feedback";
            public const string DeleteDialogTitle = "Delete Feedback";

            public const string EmployeeLabel = "Employee";
            public const string GivenByLabel = "Given By";
            public const string FeedbackTypeLabel = "Feedback Type";
            public const string ContentLabel = "Content";
            public const string VisibilityLabel = "Visibility";
            public const string FeedbackDateLabel = "Feedback Date";

            public const string ColumnEmployee = "Employee";
            public const string ColumnGivenBy = "Given By";
            public const string ColumnFeedbackType = "Feedback Type";
            public const string ColumnContent = "Content";
            public const string ColumnVisibility = "Visibility";
            public const string ColumnFeedbackDate = "Feedback Date";

            public const string SelectEmployee = "-- Select Employee --";
            public const string SelectFeedbackType = "-- Select Type --";
            public const string SelectVisibility = "-- Select Visibility --";

            public const string Created = "Feedback created successfully.";
            public const string Updated = "Feedback updated successfully.";
            public const string Deleted = "Feedback deleted successfully.";

            public const string NoDataFound = "No feedback entries found.";
        }

        public static class OneOnOneMeeting
        {
            public const string PageTitle = "One-on-One Meetings";
            public const string SearchPlaceholder = "Search by title, employee or manager...";
            public const string AddButton = "Add Meeting";
            public const string CreateDialogTitle = "Schedule One-on-One Meeting";
            public const string EditDialogTitle = "Edit Meeting";
            public const string DeleteDialogTitle = "Delete Meeting";
            public const string CancelDialogTitle = "Cancel Meeting";
            public const string CompleteDialogTitle = "Complete Meeting";
            public const string AcknowledgeDialogTitle = "Acknowledge Meeting";

            public const string TabUpcoming = "Upcoming";
            public const string TabAll = "All Meetings";

            public const string EmployeeLabel = "Employee";
            public const string ManagerLabel = "Manager";
            public const string TitleLabel = "Meeting Title";
            public const string ScheduledDateLabel = "Scheduled Date";
            public const string MeetingTypeLabel = "Meeting Type";
            public const string SummaryLabel = "Summary";
            public const string DiscussionNotesLabel = "Discussion Notes";
            public const string PrivateNotesLabel = "Private Notes";
            public const string ActionItemsLabel = "Action Items";

            public const string ColumnTitle = "Title";
            public const string ColumnEmployee = "Employee";
            public const string ColumnManager = "Manager";
            public const string ColumnScheduledDate = "Scheduled Date";
            public const string ColumnType = "Type";
            public const string ColumnStatus = "Status";

            public const string SelectEmployee = "-- Select Employee --";
            public const string SelectManager = "-- Select Manager --";

            public const string CompleteButton = "Complete Meeting";
            public const string CancelButton = "Cancel Meeting";
            public const string AcknowledgeButton = "Acknowledge";

            public const string Created = "Meeting scheduled successfully.";
            public const string Updated = "Meeting updated successfully.";
            public const string Deleted = "Meeting deleted successfully.";
            public const string Completed = "Meeting completed successfully.";
            public const string Cancelled = "Meeting cancelled successfully.";
            public const string Acknowledged = "Meeting acknowledged successfully.";

            public const string CancelConfirmFormat = "Are you sure you want to cancel '{0}'?";
            public const string AcknowledgeConfirmFormat = "Are you sure you want to acknowledge '{0}'?";
            public const string DeleteConfirmFormat = "Are you sure you want to delete '{0}'?";
            public const string NoDataFound = "No meetings found.";
            public const string NoUpcoming = "No upcoming meetings.";
        }

        public static class Notifications
        {
            public const string PageTitle = "Notifications";
            public const string TabAll = "All";
            public const string TabUnread = "Unread";
            public const string ColumnTitle = "Title";
            public const string ColumnMessage = "Message";
            public const string ColumnType = "Type";
            public const string ColumnDate = "Date";
            public const string ColumnStatus = "Status";
            public const string MarkAsRead = "Mark as Read";
            public const string Read = "Read";
            public const string Unread = "Unread";
            public const string MarkedAsRead = "Notification marked as read.";
            public const string Deleted = "Notification deleted.";
            public const string DeleteConfirmFormat = "Are you sure you want to delete '{0}'?";
            public const string NoDataFound = "No notifications found.";
            public const string NoUnread = "No unread notifications.";
            public const string ViewAll = "View All";
            public const string DropdownTitle = "Notifications";
            public const string AgoFormat = "{0} ago";
        }

        public static class PIP
        {
            public const string PageTitle = "Performance Improvement Plans";
            public const string SearchPlaceholder = "Search by employee or manager...";
            public const string AddButton = "Create PIP";
            public const string CreateDialogTitle = "Create PIP";
            public const string EditDialogTitle = "Edit PIP";
            public const string DeleteDialogTitle = "Delete PIP";
            public const string ConcludeDialogTitle = "Conclude PIP";
            public const string ExtendDialogTitle = "Extend PIP";
            public const string ObjectivesDialogTitle = "PIP Objectives";

            public const string TabActive = "Active";
            public const string TabAll = "All PIPs";

            public const string EmployeeLabel = "Employee";
            public const string ManagerLabel = "Manager";
            public const string ReasonLabel = "Reason";
            public const string StartDateLabel = "Start Date";
            public const string EndDateLabel = "End Date";
            public const string NewEndDateLabel = "New End Date";
            public const string ExtendReasonLabel = "Extension Reason";
            public const string OutcomeLabel = "Outcome";
            public const string NotesLabel = "Notes";
            public const string SuccessfulLabel = "Successful";
            public const string FailedLabel = "Failed";

            public const string ObjectiveTitleLabel = "Title";
            public const string ObjectiveSuccessCriteriaLabel = "Success Criteria";
            public const string ObjectiveDescriptionLabel = "Description";
            public const string ObjectiveStatusLabel = "Status";
            public const string ObjectiveManagerCommentLabel = "Manager Comment";

            public const string ColumnEmployee = "Employee";
            public const string ColumnManager = "Manager";
            public const string ColumnReason = "Reason";
            public const string ColumnStartDate = "Start Date";
            public const string ColumnEndDate = "End Date";
            public const string ColumnStatus = "Status";
            public const string ColumnCreatedAt = "Created";

            public const string SelectEmployee = "-- Select Employee --";
            public const string SelectManager = "-- Select Manager --";

            public const string CreateObjectiveButton = "Add Objective";
            public const string EditObjectiveButton = "Edit Objective";
            public const string ConcludeButton = "Conclude";
            public const string ExtendButton = "Extend";
            public const string ViewButton = "View";

            public const string Created = "PIP created successfully.";
            public const string Updated = "PIP updated successfully.";
            public const string Deleted = "PIP deleted successfully.";
            public const string Concluded = "PIP concluded successfully.";
            public const string Extended = "PIP extended successfully.";
            public const string ObjectiveCreated = "Objective added successfully.";
            public const string ObjectiveUpdated = "Objective updated successfully.";
            public const string ObjectiveDeleted = "Objective deleted successfully.";

            public const string DeleteConfirmFormat = "Are you sure you want to delete this PIP?";
            public const string NoDataActive = "No active PIPs.";
            public const string NoDataFound = "No PIPs found.";
        }

        public static class PositionPIPTemplate
        {
            public const string PageTitle = "Position PIP Templates";
            public const string AddButton = "Add Template";
            public const string CreateDialogTitle = "Create PIP Template";
            public const string EditDialogTitle = "Edit PIP Template";
            public const string DeleteDialogTitle = "Delete PIP Template";

            public const string PositionLabel = "Position";
            public const string TitleLabel = "Title";
            public const string SuccessCriteriaLabel = "Success Criteria";
            public const string DescriptionLabel = "Description";
            public const string ActiveLabel = "Active";

            public const string ColumnPosition = "Position";
            public const string ColumnTitle = "Title";
            public const string ColumnSuccessCriteria = "Success Criteria";
            public const string ColumnActive = "Active";

            public const string SelectPosition = "-- Select Position --";

            public const string Created = "PIP template created successfully.";
            public const string Updated = "PIP template updated successfully.";
            public const string Deleted = "PIP template deleted successfully.";

            public const string NoDataFound = "Select a position to see PIP templates.";
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
            public const string ViewKpiDialogTitle = "KPI Assignments";
            public const string NoDataFound = "Try adjusting your search or add a new employee.";
        }
    }
}

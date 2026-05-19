namespace EPMS.Shared.Constants;

public static class ServiceResponseMessages
{
    public static class AuthMsg
    {
        public const string InvalidCredentials = "Invalid email or password.";
        public const string EmailAlreadyRegistered = "Email is already registered.";
        public const string UserNotFound = "User not found.";
        public const string CurrentPasswordIncorrect = "Current password is incorrect.";
        public const string PasswordChangeFailed = "Failed to change password.";
        public const string PasswordChanged = "Password changed successfully.";
        public const string LoggedOut = "Logged out successfully.";
        public const string InvalidRefreshToken = "Invalid refresh token.";
        public const string RefreshTokenExpired = "Refresh token expired. Please login again.";
        public const string LoginSuccess = "Login successful";
        public const string TokenRefreshed = "Token refreshed successfully";
        public static string UserRegistered => "User registered successfully";
    }

    public static class DepartmentMsg
    {
        public const string Created = "Department created successfully.";
        public const string Updated = "Department updated successfully.";
        public const string Deleted = "Department deleted successfully.";
        public const string Retrieved = "Department retrieved successfully.";
        public const string RetrievedAll = "Departments retrieved successfully.";
        public const string DuplicateCode = "Department with code '{0}' already exists.";
        public const string DuplicateName = "Department with name '{0}' already exists.";
        public const string DuplicateNameOther = "Another department with name '{0}' already exists.";
        public static string NotFound(long id) => $"Department with ID '{id}' was not found.";
        public static string InUse(long id) => $"Department with ID '{id}' cannot be deleted because it has associated teams.";
    }

    public static class TeamMsg
    {
        public const string Created = "Team created successfully.";
        public const string Updated = "Team updated successfully.";
        public const string Deleted = "Team deleted successfully.";
        public const string Retrieved = "Team retrieved successfully.";
        public const string RetrievedAll = "Teams retrieved successfully.";
        public const string Added = "Team added successfully.";
        public const string Removed = "Team removed successfully.";
        public const string DuplicateCode = "Team with code '{0}' already exists.";
        public const string DuplicateName = "Team with name '{0}' already exists in this department.";
        public static string NotFound(long id) => $"Team with ID '{id}' was not found.";
        public static string NotFoundInDepartment(long teamId, long departmentId) => $"Team '{teamId}' does not belong to department '{departmentId}'.";
        public static string NotFoundForDepartment(long departmentId) => $"Department with ID '{departmentId}' was not found.";
        public static string InUse(long id) => $"Team with ID '{id}' cannot be deleted because it has assigned employees.";
    }

    public static class PositionMsg
    {
        public const string Created = "Position created successfully.";
        public const string Updated = "Position updated successfully.";
        public const string Deleted = "Position deleted successfully.";
        public const string Retrieved = "Position retrieved successfully.";
        public const string RetrievedAll = "Positions retrieved successfully.";
        public const string DuplicateCode = "Position with code '{0}' already exists.";
        public const string DuplicateName = "Position with name '{0}' already exists.";
        public const string PermissionAssigned = "Permission assigned successfully.";
        public const string PermissionRemoved = "Permission removed successfully.";
        public static string NotFound(long id) => $"Position with ID '{id}' was not found.";
        public static string LevelNotFound(long levelId) => $"Level with ID '{levelId}' was not found.";
    }

    public static class LevelMsg
    {
        public const string Created = "Level created successfully.";
        public const string Updated = "Level updated successfully.";
        public const string Deleted = "Level deleted successfully.";
        public const string Retrieved = "Level retrieved successfully.";
        public const string RetrievedAll = "Levels retrieved successfully.";
        public const string DuplicateCode = "Level with code '{0}' already exists.";
        public const string DuplicateName = "Level with name '{0}' already exists.";
        public static string NotFound(long id) => $"Level with ID '{id}' was not found.";
        public static string InUse(long id) => $"Level with ID '{id}' cannot be deleted because it is assigned to one or more positions.";
    }

    public static class EmployeeProfileMsg
    {
        public const string Created = "Employee profile created successfully.";
        public const string Updated = "Employee profile updated successfully.";
        public const string Deleted = "Employee profile deleted successfully.";
        public const string Retrieved = "Employee profile retrieved successfully.";
        public const string RetrievedAll = "Employee profiles retrieved successfully.";
        public const string DuplicateStaffNo = "Staff number '{0}' already exists.";
        public const string DuplicateEmail = "Employee with email '{0}' already exists.";
        public const string DuplicateUserId = "Employee with user ID '{0}' already exists.";
        public const string UserNotFound = "Referenced user account was not found.";
        public static string NotFound(long id) => $"Employee profile with ID '{id}' was not found.";
        public static string NotFound(Guid id) => $"Employee profile with ID '{id}' was not found.";
    }

    public static class EmployeeContactMsg
    {
        public const string Created = "Employee contact created successfully.";
        public const string Updated = "Employee contact updated successfully.";
        public const string Deleted = "Employee contact deleted successfully.";
        public const string Retrieved = "Employee contact retrieved successfully.";
        public const string RetrievedAll = "Employee contacts retrieved successfully.";
        public static string NotFound(long id) => $"Employee contact with ID '{id}' was not found.";
        public static string NotFound(Guid id) => $"Employee contact with ID '{id}' was not found.";
    }

    public static class EmployeeEmploymentMsg
    {
        public const string Created = "Employee employment created successfully.";
        public const string Updated = "Employee employment updated successfully.";
        public const string Deleted = "Employee employment deleted successfully.";
        public const string Retrieved = "Employee employment retrieved successfully.";
        public const string RetrievedAll = "Employee employments retrieved successfully.";
        public static string NotFound(long id) => $"Employee employment with ID '{id}' was not found.";
        public static string NotFound(Guid id) => $"Employment info for employee '{id}' was not found.";
    }

    public static class EmployeePayrollInfoMsg
    {
        public const string Created = "Employee payroll info created successfully.";
        public const string Updated = "Employee payroll info updated successfully.";
        public const string Deleted = "Employee payroll info deleted successfully.";
        public const string Retrieved = "Employee payroll info retrieved successfully.";
        public const string RetrievedAll = "Employee payroll info retrieved successfully.";
        public const string SalaryNegative = "Salary cannot be negative.";
        public static string NotFound(long id) => $"Employee payroll info with ID '{id}' was not found.";
        public static string NotFound(Guid id) => $"Employee payroll info for employee '{id}' was not found.";
    }

    public static class EmployeeFamilyInfoMsg
    {
        public const string Created = "Employee family info created successfully.";
        public const string Updated = "Employee family info updated successfully.";
        public const string Deleted = "Employee family info deleted successfully.";
        public const string Retrieved = "Employee family info retrieved successfully.";
        public const string RetrievedAll = "Employee family info retrieved successfully.";
        public static string NotFound(long id) => $"Employee family info with ID '{id}' was not found.";
        public static string NotFound(Guid id) => $"Family info for employee '{id}' was not found.";
    }

    public static class EmployeeSalaryHistoryMsg
    {
        public const string Retrieved = "Employee salary history retrieved successfully.";
        public const string RetrievedAll = "Employee salary histories retrieved successfully.";
        public static string NotFound(long id) => $"Employee salary history with ID '{id}' was not found.";
    }

    public static class EmployeeEmploymentHistoryMsg
    {
        public const string Retrieved = "Employee employment history retrieved successfully.";
        public const string RetrievedAll = "Employee employment histories retrieved successfully.";
        public static string NotFound(long id) => $"Employee employment history with ID '{id}' was not found.";
    }

    public static class CategoryMsg
    {
        public const string Created = "Category created successfully.";
        public const string Updated = "Category updated successfully.";
        public const string Deleted = "Category deleted successfully.";
        public const string Retrieved = "Category retrieved successfully.";
        public const string RetrievedAll = "Categories retrieved successfully.";
        public const string DuplicateCode = "Category with code '{0}' already exists.";
        public const string DuplicateName = "Category with name '{0}' already exists.";
        public const string SelfParent = "A category cannot be its own parent.";
        public static string NotFound(long id) => $"Category with ID '{id}' was not found.";
    }

    public static class RatingScaleMsg
    {
        public const string Created = "Rating scale created successfully.";
        public const string Updated = "Rating scale updated successfully.";
        public const string Deleted = "Rating scale deleted successfully.";
        public const string Deactivated = "Rating scale deactivated successfully.";
        public const string Reactivated = "Rating scale reactivated successfully.";
        public const string Retrieved = "Rating scale retrieved successfully.";
        public const string RetrievedAll = "Rating scales retrieved successfully.";
        public const string RetrievedActive = "Active rating scales retrieved successfully.";
        public const string MinGreaterThanMax = "Minimum score cannot be greater than maximum score.";
        public const string DuplicateRating = "Rating scale with rating '{0}' already exists.";
        public const string DuplicateLabel = "Rating scale with label '{0}' already exists.";
        public const string ScoreRangeOverlap = "Score range overlaps with an existing rating scale.";
        public static string NotFound(long id) => $"Rating scale with ID '{id}' was not found.";
        public static string NotFoundByRating(int rating) => $"Rating scale with rating '{rating}' was not found.";
    }

    public static class KPIWeightPriorityMsg
    {
        public const string Created = "KPI weight priority created successfully.";
        public const string Updated = "KPI weight priority updated successfully.";
        public const string Deleted = "KPI weight priority deleted successfully.";
        public const string Deactivated = "KPI weight priority deactivated successfully.";
        public const string Reactivated = "KPI weight priority reactivated successfully.";
        public const string Retrieved = "KPI weight priority retrieved successfully.";
        public const string RetrievedAll = "KPI weight priorities retrieved successfully.";
        public const string RetrievedActive = "Active KPI weight priorities retrieved successfully.";
        public const string MinGreaterThanMax = "Minimum weight cannot be greater than maximum weight.";
        public const string InvalidColorCode = "Color code must be a valid hex color code (e.g., #FF5733).";
        public const string DuplicateLevelName = "KPI weight priority with level name '{0}' already exists.";
        public static string NotFound(long id) => $"KPI weight priority with ID '{id}' was not found.";
        public static string NotFoundByLevelName(string levelName) => $"KPI weight priority with level name '{levelName}' was not found.";
    }

    public static class AppraisalCycleMsg
    {
        public const string Created = "Appraisal cycle created successfully.";
        public const string Updated = "Appraisal cycle updated successfully.";
        public const string Deleted = "Appraisal cycle deleted successfully.";
        public const string Deactivated = "Appraisal cycle deactivated successfully.";
        public const string Reactivated = "Appraisal cycle reactivated successfully.";
        public const string Locked = "Appraisal cycle locked successfully.";
        public const string Unlocked = "Appraisal cycle unlocked successfully.";
        public const string Retrieved = "Appraisal cycle retrieved successfully.";
        public const string RetrievedAll = "Appraisal cycles retrieved successfully.";
        public const string RetrievedActive = "Active appraisal cycles retrieved successfully.";
        public const string DuplicateCycle = "An appraisal cycle for year '{0}' and type '{1}' already exists.";
        public static string NotFound(long id) => $"Appraisal cycle with ID '{id}' was not found.";
        public static string AlreadyLocked = "This appraisal cycle is already locked.";
        public static string AlreadyDeactivated = "This appraisal cycle is already deactivated.";
        public static string AlreadyActive = "This appraisal cycle is already active.";
        public static string CannotDeleteLocked = "Cannot delete a locked appraisal cycle.";
        public static string CannotLockDeactivated = "Cannot lock a deactivated appraisal cycle.";
    }

    public static class KPIMasterMsg
    {
        public const string Created = "KPI master created successfully.";
        public const string Updated = "KPI master updated successfully.";
        public const string Deleted = "KPI master deleted successfully.";
        public const string Deactivated = "KPI master deactivated successfully.";
        public const string Reactivated = "KPI master reactivated successfully.";
        public const string Retrieved = "KPI master retrieved successfully.";
        public const string RetrievedAll = "KPI masters retrieved successfully.";
        public const string RetrievedActive = "Active KPI masters retrieved successfully.";
        public const string DuplicateCode = "KPI master with code '{0}' already exists.";
        public static string NotFound(long id) => $"KPI master with ID '{id}' was not found.";
    }

    public static class PIPMsg
    {
        public const string Created = "PIP created successfully.";
        public const string Updated = "PIP updated successfully.";
        public const string Deleted = "PIP deleted successfully.";
        public const string Concluded = "PIP concluded successfully.";
        public const string Extended = "PIP extended successfully.";
        public const string Retrieved = "PIP retrieved successfully.";
        public const string RetrievedAll = "PIPs retrieved successfully.";
        public const string RetrievedActive = "Active PIPs retrieved successfully.";
        public const string InvalidDateRange = "End date must be after start date.";
        public const string AlreadyConcluded = "PIP is already concluded.";
        public static string NotFound(long id) => $"PIP with ID '{id}' was not found.";
        public static string NotFoundByEmployee(long employeeId) => $"PIP for employee with ID '{employeeId}' was not found.";
    }

    public static class FormTemplateMsg
    {
        public const string Created = "Form template created successfully.";
        public const string Updated = "Form template updated successfully.";
        public const string Deleted = "Form template deleted successfully.";
        public const string Deactivated = "Form template deactivated successfully.";
        public const string Reactivated = "Form template reactivated successfully.";
        public const string Retrieved = "Form template retrieved successfully.";
        public const string RetrievedAll = "Form templates retrieved successfully.";
        public const string RetrievedActive = "Active form templates retrieved successfully.";
        public const string DuplicateName = "Form template with name '{0}' already exists.";
        public static string NotFound(long id) => $"Form template with ID '{id}' was not found.";
    }

    public static class ContinuousFeedbackMsg
    {
        public const string Created = "Feedback created successfully.";
        public const string Updated = "Feedback updated successfully.";
        public const string Deleted = "Feedback deleted successfully.";
        public const string Retrieved = "Feedback retrieved successfully.";
        public const string RetrievedAll = "Feedback retrieved successfully.";
        public static string NotFound(long id) => $"Feedback with ID '{id}' was not found.";
    }

    public static class OneOnOneMeetingMsg
    {
        public const string Created = "Meeting scheduled successfully.";
        public const string Updated = "Meeting updated successfully.";
        public const string Deleted = "Meeting deleted successfully.";
        public const string Completed = "Meeting completed successfully.";
        public const string Cancelled = "Meeting cancelled successfully.";
        public const string Acknowledged = "Meeting acknowledged successfully.";
        public const string Retrieved = "Meeting retrieved successfully.";
        public const string RetrievedAll = "Meetings retrieved successfully.";
        public const string RetrievedUpcoming = "Upcoming meetings retrieved successfully.";
        public static string NotFound(long id) => $"Meeting with ID '{id}' was not found.";
public static string AlreadyCompleted = "Meeting is already completed.";
        public static string AlreadyCancelled = "Meeting is already cancelled.";
    }

    public static class EntityKPIMsg
    {
        public const string Created = "KPI assignment added successfully.";
        public const string Updated = "KPI assignment updated successfully.";
        public const string Deleted = "KPI assignment removed successfully.";
        public const string Retrieved = "KPI assignment retrieved successfully.";
        public const string RetrievedAll = "KPI assignments retrieved successfully.";
        public static string NotFound(long id) => $"KPI assignment with ID '{id}' was not found.";
        public static string DuplicateEntry = "KPI already assigned to this entity.";
        public static string PriorityNotFound = "Priority not found.";
        public static string InvalidEntityType = "Invalid entity type. Must be Position, Department, or Team.";
        public static string WeightExceeded(decimal current, decimal newWeight) =>
            $"Total weightage ({current}%) plus new weightage ({newWeight}%) would exceed 100%.";
        public static string WeightNotComplete(decimal total) =>
            $"KPI assigned. Total weightage is currently {total}%. Consider completing to 100%.";
    }

    public static class EmployeeKPIMsg
    {
        public const string Created = "Employee KPI added successfully.";
        public const string Updated = "Employee KPI updated successfully.";
        public const string Deleted = "Employee KPI removed successfully.";
        public const string Retrieved = "Employee KPI retrieved successfully.";
        public const string RetrievedAll = "Employee KPIs retrieved successfully.";
        public static string NotFound(long id) => $"Employee KPI with ID '{id}' was not found.";
        public static string DuplicateEntry = "KPI already assigned to this employee for this cycle.";
        public static string PriorityNotFound = "Priority not found.";
        public static string WeightExceeded(decimal current, decimal newWeight) =>
            $"Total weightage ({current}%) plus new weightage ({newWeight}%) would exceed 100%.";
        public static string WeightNotComplete(decimal total) =>
            $"KPI assigned. Total weightage is currently {total}%. Consider completing to 100%.";
    }

    public static class PermissionMsg
    {
        public const string Created = "Permission created successfully.";
        public const string Updated = "Permission updated successfully.";
        public const string Deleted = "Permission deleted successfully.";
        public const string Retrieved = "Permission retrieved successfully.";
        public const string RetrievedAll = "Permissions retrieved successfully.";
        public const string DuplicateCode = "Permission code already exists.";
        public const string DuplicateName = "Permission with name '{0}' already exists.";
        public const string NotFound = "Permission not found.";
        public static string NotFoundById(long id) => $"Permission with ID '{id}' was not found.";
    }

    public static class NotificationMsg
    {
        public const string Created = "Notification sent successfully.";
        public const string Updated = "Notification updated successfully.";
        public const string Deleted = "Notification deleted successfully.";
        public const string MarkedAsRead = "Notification marked as read.";
        public const string Retrieved = "Notification retrieved successfully.";
        public const string RetrievedAll = "Notifications retrieved successfully.";
        public static string NotFound(long id) => $"Notification with ID '{id}' was not found.";
    }

    public static class DocumentAttachmentMsg
    {
        public const string Created = "Document attached successfully.";
        public const string Updated = "Document updated successfully.";
        public const string Deleted = "Document deleted successfully.";
        public const string Retrieved = "Document retrieved successfully.";
        public const string RetrievedAll = "Documents retrieved successfully.";
        public static string NotFound(long id) => $"Document with ID '{id}' was not found.";
    }

    public static class QuestionRatingScaleMsg
    {
        public const string Created = "Question rating scale created successfully.";
        public const string Updated = "Question rating scale updated successfully.";
        public const string Deleted = "Question rating scale deleted successfully.";
        public const string Retrieved = "Question rating scale retrieved successfully.";
        public const string RetrievedAll = "Question rating scales retrieved successfully.";
        public static string NotFound(long id) => $"Question rating scale with ID '{id}' was not found.";
    }

    public static class PositionPIPTemplateMsg
    {
        public const string Created = "Position PIP template created successfully.";
        public const string Updated = "Position PIP template updated successfully.";
        public const string Deleted = "Position PIP template deleted successfully.";
        public const string Retrieved = "Position PIP template retrieved successfully.";
        public const string RetrievedAll = "Position PIP templates retrieved successfully.";
        public const string RetrievedActive = "Active position PIP templates retrieved successfully.";
        public static string NotFound(long id) => $"Position PIP template with ID '{id}' was not found.";
    }

    public static class PositionFormTemplateMsg
    {
        public const string Created = "Position form template created successfully.";
        public const string Updated = "Position form template updated successfully.";
        public const string Deleted = "Position form template deleted successfully.";
        public const string Retrieved = "Position form template retrieved successfully.";
        public const string RetrievedAll = "Position form templates retrieved successfully.";
        public const string DuplicateEntry = "Position form template already exists.";
        public static string NotFound(long id) => $"Position form template with ID '{id}' was not found.";
    }

public static class PIPObjectiveMsg
    {
        public const string Created = "PIP objective created successfully.";
        public const string Updated = "PIP objective updated successfully.";
        public const string Deleted = "PIP objective deleted successfully.";
        public const string Retrieved = "PIP objective retrieved successfully.";
        public const string RetrievedAll = "PIP objectives retrieved successfully.";
        public const string RetrievedByPIP = "PIP objectives for PIP retrieved successfully.";
        public static string NotFound(long id) => $"PIP objective with ID '{id}' was not found.";
    }

    public static class PositionPermissionMsg
    {
        public const string Created = "Permission assigned to position successfully.";
        public const string Deleted = "Permission removed from position successfully.";
        public const string Retrieved = "Position permission retrieved successfully.";
        public const string RetrievedAll = "Position permissions retrieved successfully.";
        public const string RetrievedByPosition = "Permissions for position retrieved successfully.";
        public const string RetrievedByPermission = "Positions for permission retrieved successfully.";
        public static string NotFound(long id) => $"Position permission with ID '{id}' was not found.";
        public static string NotFoundByPositionAndPermission(long positionId, long permissionId) => $"Permission '{permissionId}' is not assigned to position '{positionId}'.";
        public static string DuplicateEntry = "Permission is already assigned to this position.";
    }

    public static class FormQuestionMsg
    {
        public const string Created = "Form question created successfully.";
        public const string Updated = "Form question updated successfully.";
        public const string Deleted = "Form question deleted successfully.";
        public const string Retrieved = "Form question retrieved successfully.";
        public const string RetrievedAll = "Form questions retrieved successfully.";
        public const string RetrievedByTemplate = "Form questions for template retrieved successfully.";
        public const string RetrievedByCategory = "Form questions for category retrieved successfully.";
        public static string NotFound(long id) => $"Form question with ID '{id}' was not found.";
        public static string DuplicateEntry = "Form question with this sequence already exists in the template.";
    }

    public static class AppraisalMsg
    {
        public const string Created = "Appraisal created successfully.";
        public const string Updated = "Appraisal updated successfully.";
        public const string Deleted = "Appraisal deleted successfully.";
        public const string Submitted = "Appraisal submitted successfully.";
        public const string Locked = "Appraisal locked successfully.";
        public const string Unlocked = "Appraisal unlocked successfully.";
        public const string Retrieved = "Appraisal retrieved successfully.";
        public const string RetrievedAll = "Appraisals retrieved successfully.";
        public const string RetrievedByEmployee = "Appraisals for employee retrieved successfully.";
        public const string DuplicateEntry = "An appraisal with these parameters already exists.";
        public const string UnlockReasonRequired = "An unlock reason is required.";
        public static string NotFound(long id) => $"Appraisal with ID '{id}' was not found.";
        public static string AlreadyLocked = "Appraisal is already locked.";
        public static string AlreadyUnlocked = "Appraisal is not locked.";
        public static string NotFoundByEmployee(long employeeId) => $"Appraisal for employee with ID '{employeeId}' was not found.";
    }

    public static class AppraisalRecommendationMsg
    {
        public const string Created = "Recommendation created successfully.";
        public const string Updated = "Recommendation updated successfully.";
        public const string Deleted = "Recommendation deleted successfully.";
        public const string Approved = "Recommendation approved successfully.";
        public const string Rejected = "Recommendation rejected successfully.";
        public const string Retrieved = "Recommendation retrieved successfully.";
        public const string RetrievedAll = "Recommendations retrieved successfully.";
        public const string RetrievedByAppraisal = "Recommendations for appraisal retrieved successfully.";
        public static string NotFound(long id) => $"Recommendation with ID '{id}' was not found.";
        public static string NotFoundByAppraisal(long appraisalId) => $"Recommendation for appraisal with ID '{appraisalId}' was not found.";
        public static string AlreadyProcessed = "Recommendation has already been processed.";
        public static string CannotModify = "Cannot modify a processed recommendation.";
    }

    public static class EvaluationResponseMsg
    {
        public const string Created = "Evaluation response created successfully.";
        public const string Updated = "Evaluation response updated successfully.";
        public const string Deleted = "Evaluation response deleted successfully.";
        public const string Retrieved = "Evaluation response retrieved successfully.";
        public const string RetrievedAll = "Evaluation responses retrieved successfully.";
        public const string RetrievedByAppraisal = "Evaluation responses for appraisal retrieved successfully.";
        public const string RetrievedByTemplate = "Evaluation responses for template retrieved successfully.";
        public const string RetrievedByQuestion = "Evaluation responses for question retrieved successfully.";
        public const string Submitted = "Evaluation responses submitted successfully.";
        public static string NotFound(long id) => $"Evaluation response with ID '{id}' was not found.";
        public static string NotFoundByAppraisal(long appraisalId) => $"Evaluation response for appraisal with ID '{appraisalId}' was not found.";
    }

    public static class ExcelMsg
    {
        public const string Exported = "Data exported successfully.";
        public const string Imported = "Data imported successfully.";
        public static string ExportFailed(string reason) => $"Export failed: {reason}";
        public static string ImportFailed(string reason) => $"Import failed: {reason}";
    }
}
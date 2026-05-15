namespace EPMS.Shared.Validators.ValidationMessages
{
    public static class PerformanceValidationMessages
    {
        public static class RatingScale
        {
            public const string RatingInvalid = "Rating must be greater than 0.";
            public const string LabelRequired = "Label is required.";
            public const string LabelMaxLength = "Label cannot exceed 100 characters.";
            public const string MinScoreInvalid = "Minimum score must be greater than or equal to 0.";
            public const string MaxScoreInvalid = "Maximum score must be greater than or equal to 0.";
            public const string MinScoreGreaterThanMax = "Minimum score cannot be greater than maximum score.";
            public const string PerformanceLevelMaxLength = "Performance level cannot exceed 100 characters.";
            public const string PromotionEligibilityMaxLength = "Promotion eligibility cannot exceed 100 characters.";
            public const string DescriptionMaxLength = "Description cannot exceed 500 characters.";
        }

        public static class KPIWeightPriority
        {
            public const string LevelNameRequired = "Level name is required.";
            public const string LevelNameMaxLength = "Level name cannot exceed 50 characters.";
            public const string MinWeightInvalid = "Minimum weight must be greater than or equal to 0.";
            public const string MaxWeightInvalid = "Maximum weight must be greater than or equal to 0.";
            public const string MinWeightGreaterThanMax = "Minimum weight cannot be greater than maximum weight.";
            public const string ColorCodeInvalid = "Color code must be a valid hex color code (e.g., #FF5733).";
        }

        public static class Appraisal
        {
            public const string IdRequired = "Appraisal ID is required.";
            public const string EmployeeIdRequired = "Employee is required.";
            public const string CycleIdRequired = "Appraisal cycle is required.";
            public const string AppraiserIdRequired = "Appraiser is required.";
            public const string EvaluatorIdRequired = "Evaluator ID is required.";
            public const string EvaluatorRoleRequired = "Evaluator role is required.";
            public const string EvaluatorRoleMaxLength = "Evaluator role cannot exceed 50 characters.";
            public const string DetailsRequired = "Assessment scores are required.";
            public const string RatingRange = "Rating must be between 1 and 5.";
            public const string CommentMaxLength = "Comment cannot exceed 1000 characters.";
            public const string StatusMaxLength = "Status cannot exceed 50 characters.";
        }

        public static class AppraisalRecommendation
        {
            public const string AppraisalIdRequired = "Appraisal is required.";
            public const string TypeRequired = "Recommendation type is required.";
            public const string TypeMaxLength = "Recommendation type cannot exceed 50 characters.";
            public const string ReasonRequired = "Reason is required.";
            public const string ReasonMaxLength = "Reason cannot exceed 500 characters.";
            public const string ProposedValueMaxLength = "Proposed value cannot exceed 100 characters.";
            public const string PriorityRequired = "Priority is required.";
            public const string PriorityMaxLength = "Priority cannot exceed 20 characters.";
            public const string HRCommentsMaxLength = "HR comments cannot exceed 500 characters.";
        }

        public static class ScoreRange
        {
            public const string MinimumScoreInvalid = "Minimum score must be greater than or equal to 0.";
            public const string MaximumScoreInvalid = "Maximum score must be greater than or equal to 0.";
        }

        public static class WeightRange
        {
            public const string MinimumWeightInvalid = "Minimum weight must be greater than or equal to 0.";
            public const string MaximumWeightInvalid = "Maximum weight must be greater than or equal to 0.";
        }

        public static class AppraisalCycle
        {
            public const string IdRequired = "Appraisal cycle ID is required.";
            public const string NameRequired = "Cycle name is required.";
            public const string NameMaxLength = "Cycle name cannot exceed 100 characters.";
            public const string CalendarTypeRequired = "Calendar type is required.";
            public const string CalendarTypeMaxLength = "Calendar type cannot exceed 50 characters.";
            public const string YearLabelRequired = "Year label is required.";
            public const string YearLabelMaxLength = "Year label cannot exceed 50 characters.";
            public const string AppraisalTypeRequired = "Appraisal type is required.";
            public const string AppraisalTypeMaxLength = "Appraisal type cannot exceed 50 characters.";
            public const string EvaluationStartDateRequired = "Evaluation start date is required.";
            public const string EvaluationEndDateRequired = "Evaluation end date is required.";
            public const string EvaluationEndAfterStart = "Evaluation end date must be after start date.";
            public const string WindowStartDateRequired = "Window start date is required.";
            public const string WindowEndDateRequired = "Window end date is required.";
            public const string WindowEndAfterStart = "Window end date must be after start date.";
            public const string WindowStartAfterEvaluationEnd = "Window start date must be on or after evaluation end date.";
            public const string SelfReviewStartBeforeDeadline = "Self-review start date must be before deadline.";
            public const string ManagerReviewStartBeforeDeadline = "Manager review start date must be before deadline.";
            public const string PeerReviewStartBeforeDeadline = "Peer review start date must be before deadline.";
            public const string AppraisalTypeInvalid = "Appraisal type must be one of: Annual, Semi-Annual, Quarterly, Monthly.";
            public const string CalendarTypeInvalid = "Calendar type must be one of: Standard Calendar, Fiscal Year Calendar.";
            public const string EvaluationPeriodExceedsMax = "Evaluation period ({0} days) exceeds the maximum of {1} days allowed for {2} appraisal.";
            public const string EvaluationPeriodBelowMinimum = "Evaluation period ({0} days) is below the minimum of {1} days required for {2} appraisal.";
            public const string EvaluationDateOutsideYearRange = "Evaluation date must be within {0:dd/MM/yyyy} to {1:dd/MM/yyyy} for year '{2}'.";
            public const string SelfReviewOutsideWindow = "Self-review dates must fall within the appraisal window ({0:dd/MM/yyyy} to {1:dd/MM/yyyy}).";
            public const string ManagerReviewOutsideWindow = "Manager review dates must fall within the appraisal window ({0:dd/MM/yyyy} to {1:dd/MM/yyyy}).";
            public const string PeerReviewOutsideWindow = "Peer review dates must fall within the appraisal window ({0:dd/MM/yyyy} to {1:dd/MM/yyyy}).";
        }

        public static class KPIMaster
        {
            public const string IdRequired = "KPI master ID is required.";
            public const string CategoryIdRequired = "Category is required.";
            public const string CodeRequired = "KPI code is required.";
            public const string CodeMaxLength = "KPI code cannot exceed 20 characters.";
            public const string NameRequired = "KPI name is required.";
            public const string NameMaxLength = "KPI name cannot exceed 100 characters.";
            public const string DescriptionMaxLength = "Description cannot exceed 500 characters.";
        }

        public static class PIP
        {
            public const string IdRequired = "PIP ID is required.";
            public const string EmployeeIdRequired = "Employee is required.";
            public const string ManagerIdRequired = "Manager is required.";
            public const string StartDateRequired = "Start date is required.";
            public const string EndDateRequired = "End date is required.";
            public const string EndDateAfterStart = "End date must be after start date.";
            public const string ReasonRequired = "Reason is required.";
            public const string ReasonMaxLength = "Reason cannot exceed 1000 characters.";
            public const string NotesMaxLength = "Notes cannot exceed 500 characters.";
        }

        public static class FormTemplate
        {
            public const string IdRequired = "Form template ID is required.";
            public const string NameRequired = "Form template name is required.";
            public const string NameMaxLength = "Form template name cannot exceed 100 characters.";
            public const string FormTypeRequired = "Form type is required.";
            public const string FormTypeMaxLength = "Form type cannot exceed 50 characters.";
        }

        public static class ContinuousFeedback
        {
            public const string IdRequired = "Feedback ID is required.";
            public const string EmployeeIdRequired = "Employee is required.";
            public const string GivenByIdRequired = "Given by user is required.";
            public const string FeedbackTypeRequired = "Feedback type is required.";
            public const string FeedbackTypeMaxLength = "Feedback type cannot exceed 50 characters.";
            public const string ContentRequired = "Content is required.";
            public const string ContentMaxLength = "Content cannot exceed 2000 characters.";
            public const string VisibilityRequired = "Visibility is required.";
            public const string VisibilityMaxLength = "Visibility cannot exceed 50 characters.";
        }

        public static class OneOnOneMeeting
        {
            public const string IdRequired = "Meeting ID is required.";
            public const string EmployeeIdRequired = "Employee is required.";
            public const string ManagerIdRequired = "Manager is required.";
            public const string TitleRequired = "Title is required.";
            public const string TitleMaxLength = "Title cannot exceed 200 characters.";
            public const string ScheduledDateRequired = "Scheduled date is required.";
            public const string SummaryMaxLength = "Summary cannot exceed 2000 characters.";
            public const string DiscussionNotesMaxLength = "Discussion notes cannot exceed 5000 characters.";
            public const string PrivateNotesMaxLength = "Private notes cannot exceed 2000 characters.";
            public const string ActionItemsMaxLength = "Action items cannot exceed 2000 characters.";
        }

        public static class PositionKPI
        {
            public const string IdRequired = "Position KPI ID is required.";
            public const string PositionIdRequired = "Position is required.";
            public const string KPIIdRequired = "KPI is required.";
            public const string PriorityIdRequired = "Priority is required.";
            public const string WeightageInvalid = "Weightage must be between 0 and 100.";
            public const string WeightageExceedsPriorityMax = "Weightage exceeds the maximum allowed for this priority level.";
            public const string TargetValueMaxLength = "Target value cannot exceed 100 characters.";
            public const string TargetUnitMaxLength = "Target unit cannot exceed 50 characters.";
        }

        public static class QuestionRatingScale
        {
            public const string NameRequired = "Question rating scale name is required.";
            public const string NameMaxLength = "Name cannot exceed 100 characters.";
            public const string MinScoreInvalid = "Minimum score must be greater than 0.";
            public const string MaxScoreInvalid = "Maximum score must be greater than 0.";
            public const string MaxScoreGreaterThanMin = "Maximum score must be greater than minimum score.";
        }

        public static class PositionPIPTemplate
        {
            public const string PositionIdInvalid = "Position ID must be greater than 0.";
            public const string TitleRequired = "Title is required.";
            public const string TitleMaxLength = "Title cannot exceed 200 characters.";
            public const string SuccessCriteriaRequired = "Success criteria is required.";
            public const string SuccessCriteriaMaxLength = "Success criteria cannot exceed 1000 characters.";
            public const string DescriptionMaxLength = "Description cannot exceed 500 characters.";
        }

        public static class PIPObjective
        {
            public const string PIPIdRequired = "PIP is required.";
            public const string TitleRequired = "PIP objective title is required.";
            public const string TitleMaxLength = "PIP objective title cannot exceed 200 characters.";
            public const string SuccessCriteriaRequired = "PIP objective success criteria is required.";
            public const string SuccessCriteriaMaxLength = "PIP objective success criteria cannot exceed 1000 characters.";
            public const string DescriptionMaxLength = "Description cannot exceed 500 characters.";
            public const string StatusMaxLength = "Status cannot exceed 50 characters.";
            public const string ManagerCommentMaxLength = "Manager comment cannot exceed 500 characters.";
        }

        public static class PositionFormTemplate
        {
            public const string PositionIdInvalid = "Position ID must be greater than 0.";
            public const string NameRequired = "Name is required.";
            public const string NameMaxLength = "Name cannot exceed 100 characters.";
            public const string DescriptionMaxLength = "Description cannot exceed 500 characters.";
        }

        public static class FormQuestion
        {
            public const string TemplateIdRequired = "Form template is required.";
            public const string QuestionTextRequired = "Question text is required.";
            public const string QuestionTextMaxLength = "Question text cannot exceed 500 characters.";
            public const string SequenceRequired = "Sequence is required.";
            public const string SequenceInvalid = "Sequence must be greater than 0.";
            public const string CategoryIdInvalid = "Category ID must be greater than 0.";
            public const string RatingScaleIdInvalid = "Rating scale ID must be greater than 0.";
        }

        public static class EvaluationResponse
        {
            public const string AppraisalIdRequired = "Appraisal is required.";
            public const string TemplateIdRequired = "Form template is required.";
            public const string QuestionIdRequired = "Question is required.";
            public const string EvaluatorIdRequired = "Evaluator is required.";
            public const string EvaluatorRoleRequired = "Evaluator role is required.";
            public const string EvaluatorRoleMaxLength = "Evaluator role cannot exceed 50 characters.";
            public const string RatingValueInvalid = "Rating value must be between 1 and 5.";
            public const string CommentMaxLength = "Comment cannot exceed 1000 characters.";
        }
    }
}

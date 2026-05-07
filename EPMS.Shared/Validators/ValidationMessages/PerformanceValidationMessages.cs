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
            public const string EvaluatorIdRequired = "Evaluator ID is required.";
            public const string EvaluatorRoleRequired = "Evaluator Role is required.";
            public const string DetailsRequired = "Assessment scores are required.";
            public const string RatingRange = "Rating must be between 1 and 5.";
            public const string CommentMaxLength = "Comment cannot exceed 1000 characters.";
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
            public const string NameRequired = "Cycle name is required.";
            public const string NameMaxLength = "Cycle name cannot exceed 100 characters.";
            public const string YearInvalid = "Year must be between 2000 and 2100.";
            public const string AppraisalTypeRequired = "Appraisal type is required.";
            public const string AppraisalTypeMaxLength = "Appraisal type cannot exceed 50 characters.";
            public const string StartDateRequired = "Start date is required.";
            public const string EndDateRequired = "End date is required.";
            public const string EndDateAfterStart = "End date must be after start date.";
            public const string SelfReviewStartBeforeDeadline = "Self-review start date must be before deadline.";
            public const string ManagerReviewStartBeforeDeadline = "Manager review start date must be before deadline.";
            public const string PeerReviewStartBeforeDeadline = "Peer review start date must be before deadline.";
        }
    }
}

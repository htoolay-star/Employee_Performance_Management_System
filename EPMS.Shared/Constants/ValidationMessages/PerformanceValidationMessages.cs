namespace EPMS.Shared.Constants.ValidationMessages
{
    public static class PerformanceValidationMessages
    {
        public static class Appraisal
        {
            public const string IdRequired = "Appraisal ID is required.";
            public const string EvaluatorIdRequired = "Evaluator ID is required.";
            public const string EvaluatorRoleRequired = "Evaluator Role is required.";
            public const string DetailsRequired = "Assessment scores are required.";
            public const string RatingRange = "Rating must be between 1 and 5.";
            public const string CommentMaxLength = "Comment cannot exceed 1000 characters.";
        }
    }
}

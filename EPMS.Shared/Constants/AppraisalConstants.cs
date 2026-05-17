namespace EPMS.Shared.Constants;

public static class AppraisalConstants
{
    public static class AppraisalTypes
    {
        public const string Annual = "ANNUAL";
        public const string SemiAnnual = "SEMI-ANNUAL";
        public const string Quarterly = "QUARTERLY";
        public const string Monthly = "MONTHLY";

        public static readonly HashSet<string> All = [Annual, SemiAnnual, Quarterly, Monthly];
    }

    public static class CalendarTypes
    {
        public const string Standard = "STANDARD";
        public const string FiscalYear = "FISCAL YEAR";

        public static readonly HashSet<string> All = [Standard, FiscalYear];
    }

    public static class EntityTypes
    {
        public const string Position = "POSITION";
        public const string Department = "DEPARTMENT";
        public const string Team = "TEAM";

        public static readonly HashSet<string> All = [Position, Department, Team];
    }

    public static class FormTypes
    {
        public const string SelfAssessment = "Self-Assessment Form";
        public const string ThreeSixtyFeedback = "360 Degree Feedback Form";
        public const string Appraisal = "Appraisal Form";

        public static readonly HashSet<string> All = [SelfAssessment, ThreeSixtyFeedback, Appraisal];
    }

    public static class ScoringDirections
    {
        public const string HigherIsBetter = "HigherIsBetter";
        public const string LowerIsBetter = "LowerIsBetter";

        public static readonly HashSet<string> All = [HigherIsBetter, LowerIsBetter];
    }
}

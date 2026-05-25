namespace EPMS.Shared.Constants;

public static class ContinuousFeedbackTypes
{
    public const string Recognition = "RECOGNITION";
    public const string Constructive = "CONSTRUCTIVE";
    public const string Guidance = "GUIDANCE";
    public const string GeneralNote = "GENERAL_NOTE";

    public static readonly string[] All = [Recognition, Constructive, Guidance, GeneralNote];
}

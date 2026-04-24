namespace EPMS.Domain.Common;

public static class PerformanceMapper
{
    public static (string level, string promotion) Map(int rating)
    {
        return rating switch
        {
            5 => ("Excellent", "Strongly recommended"),
            4 => ("Good", "Eligible"),
            3 => ("Meet requirement", "Possible"),
            2 => ("Needs improvement", "Not eligible"),
            1 => ("Unsatisfactory", "Not eligible"),
            _ => ("Unknown", "Unknown")
        };
    }
}
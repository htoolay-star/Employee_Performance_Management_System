using EPMS.Shared.Constants;

namespace EPMS.Shared.Utilities;

public static class KPIScoringCalculator
{
    public static decimal CalculateScore(decimal? actualValue, decimal? targetValue, string scoringDirection)
    {
        if (!actualValue.HasValue || !targetValue.HasValue || targetValue.Value <= 0)
        {
            return 0m;
        }

        var actual = actualValue.Value;
        var target = targetValue.Value;

        if (scoringDirection == AppraisalConstants.ScoringDirections.LowerIsBetter)
        {
            if (actual == 0m)
            {
                return 100m;
            }

            return Math.Min(target / actual, 1m) * 100;
        }
        else
        {
            return Math.Min(actual / target, 1m) * 100;
        }
    }

    public static decimal CalculateWeightedScore(decimal score, decimal weightage)
    {
        return Math.Round((score * weightage) / 100m, 2);
    }
}

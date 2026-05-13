using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.Constants
{
public static class EmploymentStatuses
{
    public const string Permanent = "Permanent";
    public const string Probation = "Probation";
    public const string Resigned = "Resigned";
    public const string Pending = "Pending";

    public static readonly string[] All = { Permanent, Probation, Resigned };
}
}

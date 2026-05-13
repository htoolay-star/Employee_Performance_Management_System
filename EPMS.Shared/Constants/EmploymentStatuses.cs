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

    public static class Genders
    {
        public const string Male = "Male";
        public const string Female = "Female";
        public const string Other = "Other";

        public static readonly string[] All = { Male, Female, Other };
    }

    public static class MaritalStatuses
    {
        public const string Single = "Single";
        public const string Married = "Married";
        public const string Divorced = "Divorced";
        public const string Widowed = "Widowed";
        public static readonly string[] All = { Single, Married, Divorced, Widowed };
    }

    public static class Religions
    {
        public const string Christianity = "Christianity";
        public const string Islam = "Islam";
        public const string Hinduism = "Hinduism";
        public const string Buddhism = "Buddhism";
        public const string Other = "Other";
        public static readonly string[] All = { Christianity, Islam, Hinduism, Buddhism, Other };
    }

    public static class StaffTypes
    {
        public const string FullTime = "Full-Time";
        public const string PartTime = "Part-Time";
        public const string Contract = "Contract";
        public static readonly string[] All = { FullTime, PartTime, Contract };
    }

    public static class Shift
    {
        public const string Normal = "Normal";
        public const string Morning = "Morning";
        public const string Afternoon = "Afternoon";
        public const string Night = "Night";
        public static readonly string[] All = { Normal, Morning, Afternoon, Night };
    }

    public static class Currency
    {
        public const string MMK = "MMK";
        public const string USD = "USD";
        public const string EUR = "EUR";
        public const string GBP = "GBP";
        public const string JPY = "JPY";
        public static readonly string[] All = { MMK, USD, EUR, GBP, JPY };
    }

    public static class PayType
    {
        public const string Monthly = "Monthly";
        public const string Hourly = "Hourly";
        public const string Weekly = "Weekly";
        public const string BiWeekly = "Bi-Weekly";
        public static readonly string[] All = { Monthly, Hourly, Weekly, BiWeekly };
    }

    public static class RelationWithEmergencyContact
    {
        public const string Spouse = "Spouse";
        public const string Parent = "Parent";
        public const string Sibling = "Sibling";
        public const string Friend = "Friend";
        public const string Other = "Other";
        public static readonly string[] All = { Spouse, Parent, Sibling, Friend, Other };
    }
}

using EPMS.Shared.Features;

namespace EPMS.Shared.Features.EmployeeProfiles
{
    public class EmployeeProfileQueryParameters : QueryParametersBase
    {
        public string? SearchTerm { get; set; } // Matches StaffNo or StaffName
        public long? DepartmentId { get; set; }
        public long? TeamId { get; set; }
        public long? PositionId { get; set; }
        public string? EmploymentStatus { get; set; }
    }
}

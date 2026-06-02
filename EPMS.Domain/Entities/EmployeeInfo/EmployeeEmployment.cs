using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;
using EPMS.Shared.Constants;

namespace EPMS.Domain.Entities.EmployeeInfo
{
    public class EmployeeEmployment : AuditableEntity, ISoftDeletable
    {
        private EmployeeEmployment() { }

        public EmployeeEmployment(long employeeId, long departmentId, long parentDepartmentId, long positionId, string employmentStatus)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(employmentStatus);

            EmployeeId = employeeId;
            DepartmentId = departmentId;
            ParentDepartmentId = parentDepartmentId;
            PositionId = positionId;
            EmploymentStatus = employmentStatus.Trim();
        }

        public long EmployeeId { get; private set; }

        public long DepartmentId { get; private set; }
        public long ParentDepartmentId { get; private set; }
        public long? TeamId { get; private set; }
        public long PositionId { get; private set; }
        public long? DirectManagerId { get; private set; }

        public string EmploymentStatus { get; private set; } = string.Empty;
        public string? StaffType { get; private set; }
        public int? ProbationMonth { get; private set; }

        public DateOnly? DateOfAppointment { get; private set; }
        public DateOnly? DateOfConfirmation { get; private set; }
        public DateOnly? DateOfPromotion { get; private set; }
        public DateOnly? DateOfTermination { get; private set; }
        public DateOnly? DateOfTransfer { get; private set; }
        public DateOnly? DateOfDemotion { get; private set; }
        public DateOnly? DateOfTitleChange { get; private set; }

        public string? Shift { get; private set; }
        public string? FingerPrintId { get; private set; }
        public bool MobileAttendance { get; private set; }

        public DateOnly? DateOfIncrement { get; private set; }
        public string? ProductProject { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public long? DeletedBy { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public virtual EmployeeProfile Profile { get; private set; } = null!;
        public virtual EmployeeProfile? DirectManager { get; private set; }
        public virtual Department Department { get; private set; } = null!;
        public virtual Department ParentDepartment { get; private set; } = null!;
        public virtual Team? Team { get; private set; }
        public virtual Position Position { get; private set; } = null!;

        public void ConfirmEmployment(DateOnly confirmationDate)
        {
            EmploymentStatus = EmploymentStatuses.Permanent;
            DateOfConfirmation = confirmationDate;
        }

        public void AssignProject(string? productProject)
        {
            ProductProject = productProject?.Trim();
        }

        public void LogIncrement(DateOnly incrementDate)
        {
            DateOfIncrement = incrementDate;
        }

        public void UpdateDetails(
            long departmentId, long parentDepartmentId, long positionId,
            long? teamId, long? directManagerId, string employmentStatus,
            string? staffType, int? probationMonth,
            string? shift, string? fingerPrintId, bool mobileAttendance)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(employmentStatus);

            DepartmentId = departmentId;
            ParentDepartmentId = parentDepartmentId;
            PositionId = positionId;
            TeamId = teamId;
            DirectManagerId = directManagerId;
            EmploymentStatus = employmentStatus.Trim();
            StaffType = staffType?.Trim();
            ProbationMonth = probationMonth;
            Shift = shift?.Trim();
            FingerPrintId = fingerPrintId?.Trim();
            MobileAttendance = mobileAttendance;
        }
    }
}

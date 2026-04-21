using EPMS.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.EmployeeInfo
{
    public class EmployeeProfile : IAuditableEntity
    {
        private EmployeeProfile() { }

        public EmployeeProfile(long userId, string staffNo, string firstName, string? lastName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(staffNo);
            ArgumentException.ThrowIfNullOrWhiteSpace(firstName);

            UserId = userId;
            StaffNo = staffNo.Trim().ToUpperInvariant();
            FirstName = firstName;
            LastName = lastName;
        }

        public long Id { get; private set; }

        public long UserId { get; private set; }

        public string StaffNo { get; private set; } = string.Empty;
        public string FirstName { get; private set; } = string.Empty;
        public string? LastName { get; private set; }
        public string? OtherName { get; private set; }

        public string? NRCNo { get; private set; }
        public string? Gender { get; private set; }

        public string? Race { get; private set; }
        public string? Religion { get; private set; }
        public string? Nationality { get; private set; }
        public string? BirthPlace { get; private set; }
        public string? PassportNo { get; private set; }
        public string? LabourRegistrationNo { get; private set; }

        public DateOnly? DateOfBirth { get; private set; }
        public DateOnly? PassportExpireDate { get; private set; }

        public string? WorkPermitNo { get; private set; }
        public DateOnly? WorkPermitValidDate { get; private set; }
        public DateOnly? WorkPermitExpireDate { get; private set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public virtual EmployeeEmployment? Employment { get; private set; }
        public virtual EmployeeContact? Contact { get; private set; }
        public virtual EmployeePayrollInfo? PayrollInfo { get; private set; }
        public virtual EmployeeFamilyInfo? FamilyInfo { get; private set; }

        public void UpdateDemographics(string gender, DateOnly? dateOfBirth, string nationality)
        {
            Gender = gender?.Trim();
            DateOfBirth = dateOfBirth;
            Nationality = nationality?.Trim();
        }

        public void UpdateWorkPermit(string? permitNo, DateOnly? validDate, DateOnly? expireDate)
        {
            WorkPermitNo = permitNo?.Trim().ToUpperInvariant();
            WorkPermitValidDate = validDate;
            WorkPermitExpireDate = expireDate;
        }
    }
}

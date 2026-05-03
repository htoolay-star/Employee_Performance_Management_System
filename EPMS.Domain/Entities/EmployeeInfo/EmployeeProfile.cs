using EPMS.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.EmployeeInfo
{
    public class EmployeeProfile : AuditableEntity , ISoftDeletable
    {
        private EmployeeProfile() { }

        public EmployeeProfile(long? userId, string staffNo, string firstName, string? lastName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(staffNo);
            ArgumentException.ThrowIfNullOrWhiteSpace(firstName);

            UserId = userId;
            StaffNo = staffNo.Trim().ToUpperInvariant();
            FirstName = firstName;
            LastName = lastName;
        }

        public long? UserId { get; private set; }

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

        public string? ProfilePictureUrl { get; private set; }
        public string? ProfileThumbnailUrl { get; private set; }

        public string? AdditionalData { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

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

        public void UpdateProfilePicture(string? url, string? thumbnailUrl)
        {
            ProfilePictureUrl = url?.Trim();
            ProfileThumbnailUrl = thumbnailUrl?.Trim();
        }

        public void UpdateAdditionalData(string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                AdditionalData = null;
                return;
            }

            try
            {
                using var document = JsonDocument.Parse(json);

                AdditionalData = json.Trim();
            }
            catch (JsonException)
            {
                throw new ArgumentException("AdditionalData must be a valid, structurally sound JSON string.");
            }
        }
    }
}

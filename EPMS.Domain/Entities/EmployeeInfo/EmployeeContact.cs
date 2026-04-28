using EPMS.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.EmployeeInfo
{
    public class EmployeeContact : IAuditableEntity , ISoftDeletable
    {
        private EmployeeContact() { }

        public EmployeeContact(long employeeId)
        {
            EmployeeId = employeeId;
        }

        public long EmployeeId { get; private set; }

        public string? ContactAddress { get; private set; }
        public string? PermanentAddress { get; private set; }
        public string? PhoneNo { get; private set; }
        public string? PermanentPhoneNo { get; private set; }
        public string? PresentPhoneNo { get; private set; }
        public string? EmailAddress { get; private set; }

        public string? InternalPhoneNo { get; private set; }
        public string? EmergencyMobileNo { get; private set; }
        public string? RelationWithEmergencyContact { get; private set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public virtual EmployeeProfile Profile { get; private set; } = null!;

        public void UpdatePrimaryContact(string? email, string? phone, string? contactAddress)
        {
            EmailAddress = email?.Trim();
            PhoneNo = phone?.Trim();
            ContactAddress = contactAddress?.Trim();
        }

        public void UpdateEmergencyContact(string? mobileNo, string? relation)
        {
            EmergencyMobileNo = mobileNo?.Trim();
            RelationWithEmergencyContact = relation?.Trim();
        }

        public void UpdatePhones(string? personal, string? permanent, string? present, string? internalNo)
        {
            PhoneNo = personal?.Trim();
            PermanentPhoneNo = permanent?.Trim();
            PresentPhoneNo = present?.Trim();
            InternalPhoneNo = internalNo?.Trim();
        }
    }
}

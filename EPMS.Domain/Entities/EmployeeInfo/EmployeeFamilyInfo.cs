using EPMS.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.EmployeeInfo
{
    public class EmployeeFamilyInfo : IAuditableEntity
    {
        private EmployeeFamilyInfo() { }

        public EmployeeFamilyInfo(long employeeId)
        {
            EmployeeId = employeeId;
        }

        public long EmployeeId { get; private set; }

        public string? MaritalStatus { get; private set; }
        public string? SpouseName { get; private set; }
        public string? SpouseNRCNo { get; private set; }
        public string? SpouseOccupation { get; private set; }

        public string? FatherName { get; private set; }
        public string? FatherNRCNo { get; private set; }
        public string? FatherOccupation { get; private set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public virtual EmployeeProfile Profile { get; private set; } = null!;

        public void UpdateMaritalStatus(string maritalStatus, string? spouseName, string? spouseNrc, string? spouseOccupation)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(maritalStatus);

            MaritalStatus = maritalStatus.Trim();
            SpouseName = spouseName?.Trim();
            SpouseNRCNo = spouseNrc?.Trim();
            SpouseOccupation = spouseOccupation?.Trim();
        }

        public void UpdateFatherDetails(string? fatherName, string? fatherNrc, string? fatherOccupation)
        {
            FatherName = fatherName?.Trim();
            FatherNRCNo = fatherNrc?.Trim();
            FatherOccupation = fatherOccupation?.Trim();
        }
    }
}

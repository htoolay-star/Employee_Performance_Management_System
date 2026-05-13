namespace EPMS.Client.Models.Info
{
    public class EmployeeEmploymentFormModel
    {
        public long Id { get; set; }
        public long DepartmentId { get; set; }
        public long ParentDepartmentId { get; set; }
        public long PositionId { get; set; }
        public long? TeamId { get; set; }
        public long? DirectManagerId { get; set; }
        public string EmploymentStatus { get; set; } = string.Empty;
        public string? StaffType { get; set; }
        public int? ProbationMonth { get; set; }
        public DateOnly? DateOfAppointment { get; set; }
        public DateOnly? DateOfConfirmation { get; set; }
        public DateOnly? DateOfPromotion { get; set; }

        public DateTime? DateOfAppointmentProxy
        {
            get => DateOfAppointment?.ToDateTime(TimeOnly.MinValue);
            set => DateOfAppointment = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
        }

        public DateTime? DateOfConfirmationProxy
        {
            get => DateOfConfirmation?.ToDateTime(TimeOnly.MinValue);
            set => DateOfConfirmation = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
        }

        public DateTime? DateOfPromotionProxy
        {
            get => DateOfPromotion?.ToDateTime(TimeOnly.MinValue);
            set => DateOfPromotion = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
        }
        public DateOnly? DateOfTermination { get; set; }
        public DateOnly? DateOfTransfer { get; set; }
        public DateOnly? DateOfDemotion { get; set; }
        public DateOnly? DateOfTitleChange { get; set; }
        public string? Shift { get; set; }
        public string? FingerPrintId { get; set; }
        public bool MobileAttendance { get; set; }
        public DateOnly? DateOfIncrement { get; set; }
        public string? ProductProject { get; set; }
    }
}
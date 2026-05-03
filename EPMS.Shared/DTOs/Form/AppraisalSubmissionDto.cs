using EPMS.Shared.Enums;

namespace EPMS.Shared.DTOs.Form
{
    public class AppraisalSubmissionDto
    {
        public long Id { get; set; } 
        public long EvaluatorId { get; set; }
        public string EvaluatorRole { get; set; } = string.Empty; 
        public AppraisalFormType FormType { get; set; } 
        public List<AppraisalDetailDto> Details { get; set; } = new();
    }
}
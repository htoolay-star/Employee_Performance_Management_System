namespace EPMS.Client.Models.Performance
{
    public class FormTemplateFormModel
    {
        public long Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string FormType { get; set; } = string.Empty;

        public int? QuestionsPerEvaluation { get; set; }

        public bool HasYesNo { get; set; }

        public bool HasComment { get; set; }

        public bool IsActive { get; set; } = true;
    }
}

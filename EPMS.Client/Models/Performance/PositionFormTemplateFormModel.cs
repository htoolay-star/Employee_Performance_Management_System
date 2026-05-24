namespace EPMS.Client.Models.Performance
{
    public class PositionFormTemplateFormModel
    {
        public long Id { get; set; }

        public long? PositionId { get; set; }

        public long? FormTemplateId { get; set; }

        public bool IsMandatory { get; set; } = true;
    }
}

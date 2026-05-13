using System.ComponentModel.DataAnnotations;

namespace EPMS.Client.Models.Hr
{
    public class TagFormModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Tag Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Module selection is required")]
        public string Module { get; set; } = "KPI";    }
}

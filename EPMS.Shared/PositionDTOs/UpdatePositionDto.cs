
using System.ComponentModel.DataAnnotations;

namespace EPMS.Shared.PositionDTOs
{
    public class UpdatePositionDto
    {
        [Required]
        public int PositionId { get; set; }

        [Required, StringLength(100)]
        public string PositionName { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int LevelId { get; set; }

        [Range(1, int.MaxValue)]
        public int DepartmentId { get; set; }

        public int? TeamId { get; set; }

        public bool IsActive { get; set; }
    }
}

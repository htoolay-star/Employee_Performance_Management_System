using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.DTOs.PositionDTOs
{
    public record PositionGridItemDto : PositionDto
    {
        public int RowIndex { get; init; }
    }
}

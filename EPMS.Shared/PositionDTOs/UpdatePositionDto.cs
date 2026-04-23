using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.PositionDTOs
{
    internal class UpdatePositionDto : CreatePositionDto
    {
        public bool Is_Active { get; set; } = true;
    }
    
}

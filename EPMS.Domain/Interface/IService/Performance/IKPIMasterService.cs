using EPMS.Shared.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Interface.IService.Performance
{
    public interface IKPIMasterService
    {
        // KPI အားလုံးကို ယူရန်
        Task<SuccessResponse<IEnumerable<KPIMasterDto>>> GetAllAsync();

        // ID ဖြင့် တစ်ခုချင်းစီ ယူရန်
        Task<SuccessResponse<KPIMasterDto>> GetByIdAsync(long id);

        // KPI အသစ်တစ်ခု တည်ဆောက်ရန်
        Task<SuccessResponse<long>> CreateAsync(CreateKPIMasterDto dto);

        // KPI အချက်အလက် ပြင်ဆင်ရန်
        Task<SuccessResponse> UpdateAsync(long id, UpdateKPIMasterDto dto);

        // KPI ကို ဖျက်ရန်
        Task<SuccessResponse> DeleteAsync(long id);
    }
}

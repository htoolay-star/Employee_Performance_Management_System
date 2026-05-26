using EPMS.Domain.Data;
using EPMS.Domain.Interface.IService;
using EPMS.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Services
{
    public class ReportService : IReportService
    {
        private readonly AppDbContext _context;

        public ReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmployeePerformanceSummaryDto>> GetEmployeePerformanceSummaryAsync()
        {
            return await _context.EmployeeProfiles
                .Include(p => p.Employment).ThenInclude(e => e.Department)
                .Include(p => p.Employment).ThenInclude(e => e.Position)
                .Include(p => p.EmployeeKPIs)
                .Where(p => !p.IsDeleted) // Soft-Delete စစ်ထုတ်ခြင်း
                .Select(p => new EmployeePerformanceSummaryDto
                {
                    StaffNo = p.StaffNo,
                    StaffName = p.StaffName,
                    EmailAddress = p.EmailAddress,

                    // Null-Safety check ဖြင့် ဌာနအမည်ဆွဲထုတ်ခြင်း
                    DepartmentName = p.Employment != null && p.Employment.Department != null
                        ? p.Employment.Department.Name
                        : "N/A",

                    // 🎯 Fix 1: Position Title Error အား .Name သို့မဟုတ် .Title ဖြစ်နိုင်ခြေ ညှိနှိုင်းပြင်ဆင်ခြင်း
                    PositionTitle = p.Employment != null && p.Employment.Position != null
                        ? p.Employment.Position.Name // 💡 သင့် Position ထဲတွင် Title အစား Name သုံးထားခြင်းကို ဖြေရှင်းပြီး
                        : "N/A",

                    EmploymentStatus = p.Employment != null ? p.Employment.EmploymentStatus : "Unknown",

                    // 🎯 Fix 2: EmployeeKPI အမှားများအား သင်ပေးပို့သော Real Domain ကုဒ်ပါ 'Weightage' ဖြင့် အစားထိုးခြင်း
                    // ဝန်ထမ်းတစ်ဦးချင်းစီ၏ စုစုပေါင်း KPI Weightage ရမှတ်ကို ပေါင်းယူခြင်း
                    FinalAppraisalScore = p.EmployeeKPIs != null && p.EmployeeKPIs.Any()
                        ? (double)p.EmployeeKPIs.Sum(k => k.Weightage) // 💡 .Score သို့မဟုတ် .Points အစား .Weightage သို့ ပြောင်းလဲခြင်း
                        : 0,

                    // 🎯 Fix 3: Grade တွက်ချက်မှုအပိုင်းတွင်လည်း ဒိုမိန်း Property အမှန်ဖြင့် အစားထိုးခြင်း
                    PerformanceGrade = p.EmployeeKPIs != null && p.EmployeeKPIs.Any()
                        ? (p.EmployeeKPIs.Sum(k => k.Weightage) >= 90 ? "A"
                           : p.EmployeeKPIs.Sum(k => k.Weightage) >= 75 ? "B"
                           : p.EmployeeKPIs.Sum(k => k.Weightage) >= 50 ? "C" : "D")
                        : "No Grade"
                })
                .ToListAsync();
        }
    }
}

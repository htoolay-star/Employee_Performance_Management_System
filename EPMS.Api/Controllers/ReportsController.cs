using EPMS.Domain.Interface.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;
using System.Collections.Generic;
using System.IO;

namespace EPMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IReportService _reportService; // 🎯 Service ကိုပဲ Inject လုပ်တော့မည်

        public ReportsController(IWebHostEnvironment webHostEnvironment, IReportService reportService)
        {
            _webHostEnvironment = webHostEnvironment;
            _reportService = reportService;
        }

        [HttpGet("employee-summary")]
        public async Task<IActionResult> GetEmployeeSummaryReport()
        {
            // ၁။ Logic မရှိတော့ဘဲ Service ဆီကနေ ဒေတာကို တိုက်ရိုက်တောင်းယူခြင်း
            var reportData = await _reportService.GetEmployeePerformanceSummaryAsync();

            // ၂။ .rdlc ဖိုင် လမ်းကြောင်းသတ်မှတ်ခြင်း
            string reportPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Reports", "EmployeePerformanceSummary.rdlc");

            if (!System.IO.File.Exists(reportPath))
            {
                return NotFound("RDLC template structure file not found.");
            }

            // ၃။ RDLC Engine ထဲသို့ ဒေတာထည့်သွင်းခြင်း
            LocalReport report = new LocalReport();
            report.ReportPath = reportPath;
            report.DataSources.Add(new ReportDataSource("DataSet1", reportData));

            // ၄။ PDF Render လုပ်ခြင်း
            byte[] pdfBytes = report.Render("PDF");

            // ၅။ Blazor Frontend သို့ ပေးပို့ခြင်း
            return File(pdfBytes, "application/pdf", $"EPMS_Report_{DateTime.UtcNow:yyyyMMdd}.pdf");
        }
        [HttpGet("employee-summary-excel")]
        public async Task<IActionResult> GetEmployeeSummaryExcelReport()
        {
            var reportData = await _reportService.GetEmployeePerformanceSummaryAsync();
            string reportPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Reports", "EmployeePerformanceSummary.rdlc");

            if (!System.IO.File.Exists(reportPath)) return NotFound("RDLC template not found.");

            LocalReport report = new LocalReport();
            report.ReportPath = reportPath;
            report.DataSources.Add(new ReportDataSource("DataSet1", reportData));

            // 🎯 Excel OpenXML Format အသုံးပြု၍ Render ခြင်း
            byte[] excelBytes = report.Render("EXCELOPENXML");

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = $"EPMS_Report_{DateTime.UtcNow:yyyyMMdd}.xlsx";

            return File(excelBytes, contentType, fileName);
        }
    }
}
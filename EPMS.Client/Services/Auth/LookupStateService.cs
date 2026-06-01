using EPMS.Client.Services.Hr;
using EPMS.Client.Services.Info;
using EPMS.Client.Services.Performance;
using EPMS.Client.Services.Shared;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.DTOs.PerformanceDTOs.AppraisalCycleDTOs;
using EPMS.Shared.DTOs.PerformanceDTOs.QuestionRatingScaleDTOs;

namespace EPMS.Client.Services.Auth
{
    public class LookupStateService
    {
        private readonly IDepartmentApiClient _deptApi;
        private readonly ITeamApiClient _teamApi;
        private readonly IPositionApiClient _posApi;
        private readonly ILevelApiClient _levelApi;
        private readonly ICategoryApiClient _categoryApi;
        private readonly IEmployeeProfileApiClient _employeeApi;
        private readonly IKPIMasterApiClient _kpiMasterApi;
        private readonly IQuestionRatingScaleApiClient _ratingScaleApi;
        private readonly IFormTemplateApiClient _formTemplateApi;
        private readonly IAppraisalCycleApiClient _cycleApi;

        private List<LookUpDto>? _departments;
        private List<LookUpDto>? _teams;
        private List<LookUpDto>? _positions;
        private List<LookUpDto>? _levels;
        private List<LookUpDto>? _categories;
        private List<LookUpDto>? _kpiMasters;
        private List<LookUpDto>? _ratingScales;
        private List<LookUpDto>? _formTemplates;
        private List<EmployeeLookupDto>? _employees;
        private List<AppraisalCycleDto>? _cycles;

        public LookupStateService(IDepartmentApiClient deptApi, ITeamApiClient teamApi, IPositionApiClient posApi, ILevelApiClient levelApi, ICategoryApiClient categoryApi, IEmployeeProfileApiClient employeeApi, IKPIMasterApiClient kpiMasterApi, IQuestionRatingScaleApiClient ratingScaleApi, IFormTemplateApiClient formTemplateApi, IAppraisalCycleApiClient cycleApi)
        {
            _deptApi = deptApi;
            _teamApi = teamApi;
            _posApi = posApi;
            _levelApi = levelApi;
            _categoryApi = categoryApi;
            _employeeApi = employeeApi;
            _kpiMasterApi = kpiMasterApi;
            _ratingScaleApi = ratingScaleApi;
            _formTemplateApi = formTemplateApi;
            _cycleApi = cycleApi;
        }

        public async Task<List<LookUpDto>> GetDepartmentsAsync()
        {
            if (_departments == null)
            {
                var response = await _deptApi.GetLookupAsync();
                _departments = response.Data?.ToList() ?? new List<LookUpDto>();
            }
            return _departments;
        }

        public async Task<List<LookUpDto>> GetTeamsAsync()
        {
            if (_teams == null)
            {
                var response = await _teamApi.GetLookupAsync();
                _teams = response.Data?.ToList() ?? new List<LookUpDto>();
            }
            return _teams;
        }

        public async Task<List<LookUpDto>> GetPositionsAsync()
        {
            if (_positions == null)
            {
                var response = await _posApi.GetLookupAsync();
                _positions = response.Data?.ToList() ?? new List<LookUpDto>();
            }
            return _positions;
        }

        public async Task<List<LookUpDto>> GetLevelsAsync()
        {
            if (_levels == null)
            {
                var response = await _levelApi.GetLookupAsync();
                _levels = response.Data?.ToList() ?? new List<LookUpDto>();
            }
            return _levels;
        }

        public async Task<List<LookUpDto>> GetCategoriesAsync()
        {
            if (_categories == null)
            {
                var response = await _categoryApi.GetLookupAsync();
                _categories = response.Data?.ToList() ?? new List<LookUpDto>();
            }
            return _categories;
        }

        public async Task<List<LookUpDto>> GetKPIMastersAsync()
        {
            if (_kpiMasters == null)
            {
                var response = await _kpiMasterApi.GetLookupAsync();
                _kpiMasters = response.Data?.ToList() ?? new List<LookUpDto>();
            }
            return _kpiMasters;
        }

        public async Task<List<LookUpDto>> GetRatingScalesAsync()
        {
            if (_ratingScales == null)
            {
                var response = await _ratingScaleApi.GetLookupAsync();
                _ratingScales = response.Data?.ToList() ?? new List<LookUpDto>();
            }
            return _ratingScales;
        }

        public async Task<List<LookUpDto>> GetFormTemplatesAsync()
        {
            if (_formTemplates == null)
            {
                var response = await _formTemplateApi.GetLookupAsync();
                _formTemplates = response.Data?.ToList() ?? new List<LookUpDto>();
            }
            return _formTemplates;
        }

        public async Task<List<EmployeeLookupDto>> GetEmployeesAsync()
        {
            if (_employees == null)
            {
                var response = await _employeeApi.GetLookupAsync();
                _employees = response.Data?.ToList() ?? new List<EmployeeLookupDto>();
            }
            return _employees;
        }

        public async Task<List<AppraisalCycleDto>> GetCyclesAsync()
        {
            if (_cycles == null)
            {
                var response = await _cycleApi.GetAllAsync();
                _cycles = response.Data?.ToList() ?? new List<AppraisalCycleDto>();
            }
            return _cycles;
        }

        public async Task<List<AppraisalCycleDto>> GetActiveCyclesAsync()
        {
            var all = await GetCyclesAsync();
            return all.Where(c => c.IsActive).ToList();
        }

        public void ClearDepartmentCache() => _departments = null;
        public void ClearTeamCache() => _teams = null;
        public void ClearPositionCache() => _positions = null;
        public void ClearLevelCache() => _levels = null;
        public void ClearCategoryCache() => _categories = null;
        public void ClearKPIMasterCache() => _kpiMasters = null;
        public void ClearRatingScaleCache() => _ratingScales = null;
        public void ClearFormTemplateCache() => _formTemplates = null;
        public void ClearEmployeeCache() => _employees = null;
        public void ClearCycleCache() => _cycles = null;
    }
}

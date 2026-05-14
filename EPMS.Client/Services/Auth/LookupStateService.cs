using EPMS.Client.Services.Hr;
using EPMS.Client.Services.Info;
using EPMS.Client.Services.Shared;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;

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

        private List<LookUpDto>? _departments;
        private List<LookUpDto>? _teams;
        private List<LookUpDto>? _positions;
        private List<LookUpDto>? _levels;
        private List<LookUpDto>? _categories;
        private List<EmployeeLookupDto>? _employees;

        public LookupStateService(IDepartmentApiClient deptApi, ITeamApiClient teamApi, IPositionApiClient posApi, ILevelApiClient levelApi, ICategoryApiClient categoryApi, IEmployeeProfileApiClient employeeApi)
        {
            _deptApi = deptApi;
            _teamApi = teamApi;
            _posApi = posApi;
            _levelApi = levelApi;
            _categoryApi = categoryApi;
            _employeeApi = employeeApi;
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

        public async Task<List<EmployeeLookupDto>> GetEmployeesAsync()
        {
            if (_employees == null)
            {
                var response = await _employeeApi.GetLookupAsync();
                _employees = response.Data?.ToList() ?? new List<EmployeeLookupDto>();
            }
            return _employees;
        }

        public void ClearDepartmentCache() => _departments = null;
        public void ClearTeamCache() => _teams = null;
        public void ClearPositionCache() => _positions = null;
        public void ClearLevelCache() => _levels = null;
        public void ClearCategoryCache() => _categories = null;
        public void ClearEmployeeCache() => _employees = null;
    }
}

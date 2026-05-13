using EPMS.Client.Services.Hr;
using EPMS.Shared.DTOs.Common;

namespace EPMS.Client.Services.Auth
{
    public class LookupStateService
    {
        private readonly IDepartmentApiClient _deptApi;
        private readonly ITeamApiClient _teamApi;
        private readonly IPositionApiClient _posApi;
        private readonly ILevelApiClient _levelApi;

        private List<LookUpDto>? _departments;
        private List<LookUpDto>? _teams;
        private List<LookUpDto>? _positions;
        private List<LookUpDto>? _levels;

        public LookupStateService(IDepartmentApiClient deptApi, ITeamApiClient teamApi, IPositionApiClient posApi, ILevelApiClient levelApi)
        {
            _deptApi = deptApi;
            _teamApi = teamApi;
            _posApi = posApi;
            _levelApi = levelApi;
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

        public void ClearDepartmentCache() => _departments = null;
        public void ClearTeamCache() => _teams = null;
        public void ClearPositionCache() => _positions = null;
        public void ClearLevelCache() => _levels = null;
    }
}

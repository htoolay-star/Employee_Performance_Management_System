namespace EPMS.Shared.Features.Teams
{
    public class TeamQueryParameters : QueryParametersBase
    {
        public string? SearchTerm { get; set; }
        public long? DepartmentId { get; set; }
    }
}
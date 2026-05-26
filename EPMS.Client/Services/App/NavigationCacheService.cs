using EPMS.Shared.DTOs.FormDTOs;

namespace EPMS.Client.Services.App;

public class NavigationCacheService
{
    public bool NavLoaded { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsSA { get; set; }
    public bool IsManager { get; set; }
    public HashSet<string> Permissions { get; set; } = new();
    public List<AppraisalDto>? KpiForms { get; set; }
    public List<MyEvaluationFormDto>? AppraisalForms { get; set; }

    public void Invalidate()
    {
        NavLoaded = false;
        KpiForms = null;
        AppraisalForms = null;
    }
}

using EPMS.Domain.Contracts;
using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Api.Jobs;

public class NightlyMaintenanceJob
{
    private readonly IServiceScopeFactory _scopeFactory;

    public NightlyMaintenanceJob(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task RunAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var appraisalService = scope.ServiceProvider.GetRequiredService<IAppraisalService>();

        await AutoGenerateAppraisalsAsync(appraisalService, uow);
        await AutoLockCyclesAsync(uow);
        await AutoLockRolesAsync(uow);
        await PurgeRecycleBinAsync(db);
    }

    private static async Task AutoGenerateAppraisalsAsync(IAppraisalService appraisalService, IUnitOfWork uow)
    {
        var now = DateOnly.FromDateTime(DateTime.UtcNow);
        var cycles = await uow.Perf.AppraisalCycles.GetAllAsync();
        var toGenerate = cycles.Where(c => c.IsActive && !c.IsLocked
                                       && c.WindowStartDate <= now).ToList();

        foreach (var cycle in toGenerate)
            await appraisalService.AutoGenerateForCycleAsync(cycle.Id);
    }

    private static async Task AutoLockCyclesAsync(IUnitOfWork uow)
    {
        var now = DateOnly.FromDateTime(DateTime.UtcNow);
        var cycles = await uow.Perf.AppraisalCycles.GetAllAsync();
        var toLock = cycles.Where(c => !c.IsLocked && c.IsActive && c.WindowEndDate <= now).ToList();

        if (toLock.Count == 0) return;

        var entityKPIs = (await uow.Perf.EntityKPIs.GetAllAsync()).ToList();
        var employeeKPIs = (await uow.Perf.EmployeeKPIs.GetAllAsync()).ToList();
        var snapshotDate = DateTimeOffset.UtcNow;

        foreach (var cycle in toLock)
        {
            cycle.LockCycle();
            uow.Perf.AppraisalCycles.Update(cycle);

            foreach (var kpi in entityKPIs)
            {
                uow.Perf.EntityKPIHistories.Add(new EntityKPIHistory(
                    kpi.EntityType, kpi.EntityId, cycle.Id,
                    kpi.KPIId, kpi.PriorityId, kpi.Weightage,
                    kpi.TargetValue, kpi.TargetUnit, snapshotDate
                ));
            }

            foreach (var kpi in employeeKPIs)
            {
                uow.Perf.EmployeeKPIHistories.Add(new EmployeeKPIHistory(
                    kpi.EmployeeId, cycle.Id,
                    kpi.KPIId, kpi.PriorityId, kpi.Weightage,
                    kpi.TargetValue, kpi.TargetUnit, snapshotDate
                ));
            }

            var appraisals = await uow.Perf.Appraisals.FindAllAsync(
                a => a.CycleId == cycle.Id && !a.IsDeleted,
                trackChanges: true);

            foreach (var appraisal in appraisals)
            {
                if (!appraisal.KpiLocked)
                    appraisal.LockKpi(isDeadline: true);

                if (!appraisal.SelfLocked)
                {
                    appraisal.LockSelf(isDeadline: true);
                    var selfResponses = await uow.Perf.EvaluationResponses
                        .FindAllAsync(r => r.AppraisalId == appraisal.Id
                                        && r.EvaluatorRole == EvaluatorRoles.Self
                                        && !r.IsDeleted && !r.SubmittedAt.HasValue,
                                      trackChanges: true);
                    foreach (var r in selfResponses)
                        r.Submit(TimeProvider.System);
                }

                if (!appraisal.ThreeSixtyLocked)
                {
                    appraisal.LockThreeSixty(isDeadline: true);
                    var threeSixtyResponses = await uow.Perf.EvaluationResponses
                        .FindAllAsync(r => r.AppraisalId == appraisal.Id
                                        && (r.EvaluatorRole == EvaluatorRoles.Manager
                                         || r.EvaluatorRole == EvaluatorRoles.Peer
                                         || r.EvaluatorRole == EvaluatorRoles.Subordinate)
                                        && !r.IsDeleted && !r.SubmittedAt.HasValue,
                                      trackChanges: true);
                    foreach (var r in threeSixtyResponses)
                        r.Submit(TimeProvider.System);
                }

                if (!appraisal.AppraisalLocked)
                {
                    appraisal.LockAppraisal(isDeadline: true);
                    var appraisalResponses = await uow.Perf.EvaluationResponses
                        .FindAllAsync(r => r.AppraisalId == appraisal.Id
                                        && r.EvaluatorRole == EvaluatorRoles.Appraisal
                                        && !r.IsDeleted && !r.SubmittedAt.HasValue,
                                      trackChanges: true);
                    foreach (var r in appraisalResponses)
                        r.Submit(TimeProvider.System);
                }
            }
        }

        await uow.CompleteAsync();
    }

    private static async Task AutoLockRolesAsync(IUnitOfWork uow)
    {
        var now = DateOnly.FromDateTime(DateTime.UtcNow);
        var cycles = await uow.Perf.AppraisalCycles.GetAllAsync();
        var activeCycles = cycles.Where(c => c.IsActive && !c.IsLocked).ToList();
        if (activeCycles.Count == 0) return;

        foreach (var cycle in activeCycles)
        {
            var appraisals = await uow.Perf.Appraisals.FindAllAsync(
                a => a.CycleId == cycle.Id && !a.IsDeleted,
                trackChanges: true);

            foreach (var appraisal in appraisals)
            {
                var selfDeadline = cycle.SelfReviewDeadline ?? cycle.WindowEndDate;
                if (!appraisal.SelfLocked && selfDeadline <= now)
                {
                    appraisal.LockSelf(isDeadline: true);
                    var selfResponses = await uow.Perf.EvaluationResponses
                        .FindAllAsync(r => r.AppraisalId == appraisal.Id && r.EvaluatorRole == EvaluatorRoles.Self
                                        && !r.IsDeleted && !r.SubmittedAt.HasValue,
                                      trackChanges: true);
                    foreach (var r in selfResponses)
                        r.Submit(TimeProvider.System);
                }

                var kpiDeadline = cycle.KpiReviewDeadline ?? cycle.WindowEndDate;
                if (!appraisal.KpiLocked && kpiDeadline <= now)
                {
                    appraisal.LockKpi(isDeadline: true);
                }

                var threeSixtyDeadline = cycle.ThreeSixtyReviewDeadline ?? cycle.WindowEndDate;
                if (!appraisal.ThreeSixtyLocked && threeSixtyDeadline <= now)
                {
                    appraisal.LockThreeSixty(isDeadline: true);
                    var threeSixtyResponses = await uow.Perf.EvaluationResponses
                        .FindAllAsync(r => r.AppraisalId == appraisal.Id
                                        && (r.EvaluatorRole == EvaluatorRoles.Manager || r.EvaluatorRole == EvaluatorRoles.Peer || r.EvaluatorRole == EvaluatorRoles.Subordinate)
                                        && !r.IsDeleted && !r.SubmittedAt.HasValue,
                                      trackChanges: true);
                    foreach (var r in threeSixtyResponses)
                        r.Submit(TimeProvider.System);
                }

                var appraisalDeadline = cycle.ManagerReviewDeadline ?? cycle.WindowEndDate;
                if (!appraisal.AppraisalLocked && appraisalDeadline <= now)
                {
                    appraisal.LockAppraisal(isDeadline: true);
                    var appraisalResponses = await uow.Perf.EvaluationResponses
                        .FindAllAsync(r => r.AppraisalId == appraisal.Id
                                        && r.EvaluatorRole == EvaluatorRoles.Appraisal
                                        && !r.IsDeleted && !r.SubmittedAt.HasValue,
                                      trackChanges: true);
                    foreach (var r in appraisalResponses)
                        r.Submit(TimeProvider.System);
                }
            }
        }

        await uow.CompleteAsync();
    }

    private static async Task PurgeRecycleBinAsync(AppDbContext db)
    {
        var cutoff = DateTimeOffset.UtcNow.AddDays(-30);

        var tables = new[]
        {
            ("perf.AppraisalCycles", "IsDeleted", "DeletedAt"),
            ("perf.KPIWeightPriorities", "IsDeleted", "DeletedAt"),
            ("perf.QuestionRatingScales", "IsDeleted", "DeletedAt"),
            ("perf.KPIMaster", "IsDeleted", "DeletedAt"),
            ("perf.PIPs", "IsDeleted", "DeletedAt"),
            ("perf.PIPObjectives", "IsDeleted", "DeletedAt"),
            ("perf.FormTemplates", "IsDeleted", "DeletedAt"),
            ("perf.FormQuestions", "IsDeleted", "DeletedAt"),
            ("perf.ContinuousFeedbacks", "IsDeleted", "DeletedAt"),
            ("perf.OneOnOneMeetings", "IsDeleted", "DeletedAt"),
            ("perf.EntityKPIs", "IsDeleted", "DeletedAt"),
            ("perf.EmployeeKPIs", "IsDeleted", "DeletedAt"),
            ("hr.Positions", "IsDeleted", "DeletedAt"),
            ("hr.Levels", "IsDeleted", "DeletedAt"),
            ("hr.Departments", "IsDeleted", "DeletedAt"),
            ("hr.Teams", "IsDeleted", "DeletedAt"),
            ("hr.RatingScales", "IsDeleted", "DeletedAt"),
            ("shared.Categories", "IsDeleted", "DeletedAt"),
            ("shared.DocumentAttachments", "IsDeleted", "DeletedAt")
        };

        foreach (var (table, delFlag, delAt) in tables)
        {
#pragma warning disable EF1002 // table names are hardcoded constants, not user input
            await db.Database.ExecuteSqlRawAsync(
                $"DELETE FROM {table} WHERE {delFlag} = 1 AND {delAt} < {{0}}", cutoff);
#pragma warning restore EF1002
        }
    }
}

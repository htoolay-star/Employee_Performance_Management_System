using EPMS.Domain.Contracts;
using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
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

        await AutoLockCyclesAsync(uow);
        await PurgeRecycleBinAsync(db);
    }

    private static async Task AutoLockCyclesAsync(IUnitOfWork uow)
    {
        var now = DateOnly.FromDateTime(DateTime.UtcNow);
        var cycles = await uow.Perf.AppraisalCycles.GetAllAsync();
        var toLock = cycles.Where(c => !c.IsLocked && c.IsActive && c.WindowStartDate <= now).ToList();

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

# EPMS — Appraisal Auto-Create & Auto-Finalize Flow

## Overview

```
EvaluationStartDate
  ↓ (Phase 1 & 2)
Auto-create Appraisals + populate KPI details + prepare forms for all employees

Between StartDate and WindowEndDate
  → Users fill KPIs, Self, Peer, Manager forms
  → Manager can manually create for late joiners

WindowEndDate
  ↓ (Phase 3)
Auto-finalize → aggregate component scores → composite TotalScore → lock
```

---

## Data Model (Appraisal Entity)

| Component | Entity | Feeds → |
|---|---|---|
| **KPI** | `AppraisalDetail` (`KPIId` not null) | `kpiScore` (50%) |
| **Self-Assessment** | `EvaluationResponse` (`EvaluatorRole = "SELF"`) | `selfScore` (15%) |
| **Peer/360** | `EvaluationResponse` (`EvaluatorRole = "PEER"`) | `peerScore` (10%) |
| **Manager Review** | `EvaluationResponse` (`EvaluatorRole = "MANAGER"`) | `managerScore` (25%) |

**Composite formula** (already in `Appraisal.FinalizeAppraisal()`):
```
TotalScore = (kpiScore × 0.50) + (selfScore × 0.15) + (peerScore × 0.10) + (managerScore × 0.25)
```

---

## Phase 1: Date Validation + KPI Resolution

### AppraisalService.CreateAsync

**New checks** (before existing validation):
1. `UtcNow < cycle.EvaluationStartDate` → fail `"Evaluation period hasn't started yet."`
2. `UtcNow > cycle.WindowEndDate` → fail `"The appraisal window has ended."`
3. `cycle.IsLocked` → fail `"Cycle is locked."`

**New private method** `ResolveAndAddKPIDetails(Appraisal, cycleId)`:

```
1. Load EmployeeEmployment (for PositionId, DepartmentId, TeamId)
2. Fetch EmployeeKPI records for this employee + cycle
3. Fetch EntityKPI records for:
   - ("POSITION", PositionId)
   - ("DEPARTMENT", DepartmentId)
   - ("TEAM", TeamId) if employee has a team
4. Merge (EmployeeKPI overrides EntityKPI for same KPIId)
5. For each resolved KPI:
   new AppraisalDetail(appraisalId, kpiId, kpiName, categoryName, weightage, targetValue, employeeKPIId: employeeKPI?.Id)
   appraisal.AddDetail(detail)
```

### Existing repository methods (all available)

| Method | Repository |
|---|---|
| `GetByEmployeeIdAsync(id)` → `EmployeeEmployment?` | `IEmployeeEmploymentRepository` |
| `GetByEmployeeAndCycleAsync(empId, cycleId)` → `IEnumerable<EmployeeKPI>` | `IEmployeeKPIRepository` |
| `GetByEntityAsync(entityType, entityId)` → `IEnumerable<EntityKPI>` | `IEntityKPIRepository` |

### New on `AppraisalDetail` entity

- Add constructor overload or setter for `EmployeeKPIId` (field already exists at line 37, just not set by current constructor).

---

## Phase 2: Background Service — Auto-create at EvaluationStartDate

### New file: `EPMS.Domain/Services/Performance/AppraisalAutoCreationService.cs`

```csharp
public class AppraisalAutoCreationService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcessCyclesAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
```

**Logic** (runs hourly):

```
For each cycle where:
  EvaluationStartDate ≤ UtcNow ≤ WindowEndDate
  AND IsActive = true AND IsLocked = false AND IsDeleted = false

  For each employee in EmployeeProfile:
    If NOT appraisal exists for this employee + cycle:
      → Determine ManagerReviewerId = EmployeeEmployment.DirectManagerId
      → Create new Appraisal(employeeId, cycleId, managerReviewerId)
      → Call ResolveAndAddKPIDetails(appraisal, cycleId)
      → Save
```

### DI Registration

In `Program.cs`:
```csharp
builder.Services.AddHostedService<AppraisalAutoCreationService>();
```

---

## Phase 3: Background Service — Auto-finalize at WindowEndDate

Same `AppraisalAutoCreationService`, new logic block:

**WindowEndDate trigger** (runs hourly):

```
For each cycle where:
  WindowEndDate < UtcNow
  AND IsActive = true AND IsLocked = false AND IsDeleted = false

  For each unlocked Appraisal in this cycle:
    1. kpiScore   = avg of Details.WeightedScore WHERE KPIId IS NOT NULL
    2. selfScore  = avg of Responses.RatingValue WHERE EvaluatorRole = "SELF"
    3. peerScore  = avg of Responses.RatingValue WHERE EvaluatorRole = "PEER"
    4. managerScore = avg of Responses.RatingValue WHERE EvaluatorRole = "MANAGER"
    5. Match TotalScore against RatingScale to get FinalRating
    6. Call appraisal.FinalizeAppraisal(kpiScore, selfScore, peerScore, managerScore, matchingScale, timeProvider)
    → Save
```

### Repository additions

- `IAppraisalRepository` — add `GetUnlockedByCycleIdAsync(long cycleId)` (or use base `FindAllAsync(a => a.CycleId == cycleId && !a.IsLocked)`).

---

## Files Summary

| File | Action |
|---|---|
| `AppraisalService.cs` | Add date validation + KPI resolution |
| `AppraisalAutoCreationService.cs` | **New** — background service (auto-create + auto-finalize) |
| `IAppraisalRepository.cs` | Add `GetUnlockedByCycleIdAsync(long cycleId)` |
| `AppraisalDetail.cs` | Add `EmployeeKPIId` support in constructor |
| `Program.cs` | Register `AddHostedService<AppraisalAutoCreationService>()` |

**No new entities, no DTO changes, no migration needed.**

---

## Open Questions

1. **Manager auto-assign** — use `DirectManagerId` from `EmployeeEmployment`, or leave null for manual assignment?
2. **Late joiners** — auto-pick up employees hired after `EvaluationStartDate` on next hourly run?
3. **Form questions** — this plan covers KPI detail population only. Should form questions (from `PositionFormTemplate`) also be auto-populated in this phase, or separate step?
4. **Cycle lock check on Create** — currently `CreateAsync` doesn't check if cycle is locked. Add this?

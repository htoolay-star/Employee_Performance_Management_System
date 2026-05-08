# EPMS - Next Steps & Development Plan

## Project Status Snapshot

| Module | % Complete | Done | Missing / Pending |
| :--- | :--- | :--- | :--- |
| **Security & IAM** | 85% | JWT, Refresh Tokens, Basic RBAC, PBAC Foundations. | Granular Permission checks in some controllers, Password complexity enforcement. |
| **Core HR** | 95% | Departments, Levels, Positions, Teams CRUD. | Soft-delete validation for related entities. |
| **Employee Info** | 90% | Profile, Contact, Employment, Payroll, Salary History. | Comprehensive "Full Profile" view API. |
| **Appraisal Workflow** | 60% | Appraisal/Cycle CRUD, Lock/Unlock, Detail Evaluation. | **Workflow Routing logic**, 360 Feedback dispatch, Final Score weighting (BR-01). |
| **Performance Engine** | 40% | KPI Master, Weight/Priority setup. | **Auto-PIP Trigger (BR-02)**, KPI History tracking, Scoring formula implementation. |
| **360° Feedback** | 20% | EvaluationResponse entities. | **Rater Cohort resolution**, Anonymous feedback sanitization, Radar Chart data API. |
| **PIP** | 50% | PIP/Objective CRUD. | **Auto-trigger from Appraisal**, Status history tracking, PIP Template mapping. |
| **Reporting** | 10% | Audit Log Infrastructure. | All performance dashboards, Departmental trends, HiPo identification. |

---

## Epic Traceability Summary

| Epic | User Stories | Status | Summary |
| :--- | :--- | :--- | :--- |
| **1. Security & IAM** | 13 | 🟢 Complete | Core auth is robust. |
| **2. Core HR** | 11 | 🟢 Complete | Org mapping is fully functional. |
| **3. Appraisal Workflow** | 23 | 🟡 In Progress | Basic CRUD exists; Routing & State Machine need refinement. |
| **4. Performance Engine** | 13 | 🔴 Pending | Scoring formulas and weighting (BR-01) need implementation. |
| **5. 360° Feedback** | 21 | 🔴 Pending | Rater cohort logic and anonymity features are missing. |
| **6. PIP** | 9 | 🟡 In Progress | CRUD is done; Workflow integration (BR-02) is missing. |
| **7. Reporting** | 11 | 🔴 Pending | Dashboards and analytical queries are not yet started. |

---

## Prioritized Roadmap

### Sprint 1: Performance Core & Scoring (High Priority)
- **Goal**: Implement mandatory business rules and the scoring engine.
- **Tasks**:
  - Implement **BR-01** weighted scoring formula (50/25/15/10) in `AppraisalService`.
  - Implement **BR-02** Auto-PIP trigger when score < 60.
  - Implement **BR-03** Deadline-based auto-locking mechanism (Background Service).
  - **Acceptance**: Appraisal submission correctly calculates total score and triggers PIP if necessary.

### Sprint 2: 360° Feedback & Rater Logic
- **Goal**: Automate the 360-degree evaluation process.
- **Tasks**:
  - Develop `RaterService` to resolve Peer/Subordinate cohorts based on position levels.
  - Implement cohort size checks (min 3) and randomization (max 5) for anonymity.
  - Create API for sanitized (anonymous) qualitative feedback retrieval.
  - **Acceptance**: System auto-assigns raters and displays aggregated anonymous scores.

### Sprint 3: Workflow Routing & Notifications
- **Goal**: Connect the appraisal lifecycle with automated routing.
- **Tasks**:
  - Implement "Submit to Manager" and "Route to Dept Head" logic based on Org Chart.
  - Integrate `NotificationService` for appraisal triggers, PIP alerts, and deadlines.
  - Implement 1-on-1 meeting workflow: "Start" (In-Progress) and Action Item tracking.
  - **Acceptance**: Appraisal forms route correctly through levels; users receive alerts.

### Sprint 4: Reporting, Analytics & UI Completion
- **Goal**: Deliver visibility and a functional frontend.
- **Tasks**:
  - Develop Reporting API for "High-Potential" talent and departmental trends.
  - Build core Blazor dashboards for Employee, Manager, and HR Admin.
  - Implement Radar Chart data endpoints for Perception Gap analysis.
  - **Acceptance**: HR can view org-wide performance trends; Employees see development charts.

---

## Cross-Cutting Concerns to Fix
1.  **Auth Pattern Alignment**: Ensure `EmployeeInfo` and `Shared` controllers fully inherit `ApiControllerBase` and use `HandleResult`.
2.  **PBAC & Ownership Logic**: Enforce "Department-only" visibility in Repository global query filters.
3.  **Audit Trail**: Verify all Performance entities are decorated with `AuditableEntity`.
4.  **FluentValidation**: Increase coverage for `AppraisalSubmissionDto` and `PIP` requests.

---

## Definition of Done (DoD)
- [ ] Code follows the established **Auth Pattern** (SuccessResponse, HandleResult).
- [ ] Unit tests cover core business logic (Scoring, PIP trigger).
- [ ] API endpoints are documented in Swagger.
- [ ] Permissions are correctly applied via `[Authorize(Roles = ...)]` or PBAC.
- [ ] Database migrations are created and tested.

---

## Risk Register

| Risk | Impact | Mitigation Strategy |
| :--- | :--- | :--- |
| **Scoring Formula Complexity** | High | Create dedicated unit tests for the weighting engine with edge-case scores. |
| **Anonymity Breaches** | High | Strictly enforce the "min 3 raters" rule and sanitize text feedback in the DB query layer. |
| **Deadline Race Conditions** | Medium | Use a robust background worker (Hangfire or Quartz.NET) for the auto-lock mechanism. |
| **PBAC Scope Leaks** | Medium | Implement Global Query Filters in EF Core to restrict data by Department/Team ID. |
| **Performance on Large Reports** | Low | Use Dapper or optimized EF Core Projections for reporting queries. |

---

## Testing Strategy
- **Unit Testing**: Focus on `AppraisalService` (scoring) and `RaterService` (cohort logic) using xUnit and Moq.
- **Integration Testing**: Test the full Appraisal -> Score -> PIP workflow against a test database.
- **E2E Testing**: Verify the Blazor UI flows for appraisal submission and manager approval.

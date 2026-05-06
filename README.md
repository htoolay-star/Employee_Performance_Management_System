# EPMS Project Structure

## Overview
**EPMS (Employee Performance Management System)** - .NET 9.0 full-stack web app with Clean Architecture.

## Solution Structure

```
EPMS_Solution/
├── EPMS.Api/           # ASP.NET Core Web API (Presentation)
├── EPMS.Client/        # Blazor WebAssembly SPA
├── EPMS.Domain/        # Domain Layer (Entities, Services, Data)
├── EPMS.Shared/        # Shared Library (DTOs, Validators, Constants)
└── EPMS_Solution.slnx  # Solution file
```

## Layer Details

### EPMS.Api (`@EPMS.Api/`)
| Component | Purpose |
|-----------|---------|
| `Controllers/` | REST endpoints (Auth, Hr, Performance, Shared) |
| `Extensions/` | DI registration, Auth, Database, WebAPI config |
| `AutoMapper/` | DTO-to-Entity mapping profiles |
| `Filters/` | Validation, GlobalResponse, MustChangePassword |
| `Middlewares/` | GlobalExceptionHandler |
| `Factories/` | AppDbContextFactory |

### EPMS.Domain (`@EPMS.Domain/`)
| Component | Purpose |
|-----------|---------|
| `Entities/` | Domain models organized by schema |
| `Data/` | AppDbContext, Configurations, Migrations, Seeding |
| `Services/` | Business logic (App, Auth, Hr, Performance, Shared) |
| `Interface/` | IService (15), Irepo (26) - contracts |
| `Repository/` | Repository implementations (29 repos) |
| `Contracts/` | BaseEntity, AuditableEntity, ISoftDeletable, IUnitOfWork |
| `Factories/` | AuditLogFactory, DocumentAttachmentFactory |

### EPMS.Shared (`@EPMS.Shared/`)
| Component | Purpose |
|-----------|---------|
| `DTOs/` | Data transfer objects (Auth, HR, Form, Tag, Team, etc.) |
| `Validators/` | FluentValidation rules (22 validators) |
| `Constants/` | AppClaims, Statuses, ErrorMessages, CacheKeys, Roles |
| `Enums/` | ErrorType, UserRole |
| `Models/` | Shared models |

### EPMS.Client (`@EPMS.Client/`)
| Component | Purpose |
|-----------|---------|
| `Pages/` | Blazor pages (Home, Counter, Weather) |
| `Layout/` | MainLayout, NavMenu |
| `wwwroot/` | Static assets |
| `Program.cs` | WebAssembly host configuration |

---

## Entity Schemas (`@EPMS.Domain/Entities/`)

### Auth
- `User`, `Role`, `Permission`, `UserRefreshToken`, `PositionPermission`

### HR
- `Department`, `Level`, `Position`, `Team`, `RatingScale`

### EmployeeInfo
- `EmployeeProfile`, `EmployeeContact`, `EmployeeEmployment`, `EmployeeEmploymentHistory`
- `EmployeeFamilyInfo`, `EmployeePayrollInfo`, `EmployeeSalaryHistory`

### Performance (Core)
- `Appraisal` - Performance review with status workflow
- `AppraisalCycle`, `AppraisalDetail`, `AppraisalRecommendation`, `AppraisalStatusHistory`
- `FormTemplate`, `FormQuestion`, `QuestionRatingScale`, `PositionFormTemplate`
- `EvaluationResponse`, `ContinuousFeedback`, `OneOnOneMeeting`
- `KPIMaster`, `PositionKPI`, `PositionKPIHistory`, `KPIWeightPriority`
- `PIP`, `PIPObjective`, `PIPStatusHistory`, `PositionPIPTemplate`

### Shared
- `Category`, `Tag`, `DocumentAttachment`

### App/Audit
- `Notification`, `SystemSetting`, `AuditLog`

---

## Tech Stack

| Technology | Version | Purpose |
|------------|---------|---------|
| .NET | 9.0 | Framework |
| EF Core | 9.0.0 | ORM |
| SQL Server | - | Database |
| JWT Bearer | 9.0.2 | Authentication |
| AutoMapper | 16.1.1 | Mapping |
| FluentValidation | 11.11.0 | Validation |
| BCrypt | 4.0.3 | Password hashing |
| Scrutor | 6.0.1 | Assembly scanning |
| Swashbuckle | 6.6.2 | API docs |

---

## Key Patterns

- **Clean Architecture**: 4-layer separation (Api/Client/Domain/Shared)
- **Repository + Unit of Work**: Data access abstraction
- **CQRS-ready**: Separate service interfaces
- **Soft Delete**: Global query filter via `ISoftDeletable`
- **Audit Trail**: `AuditableEntity` base class
- **Domain-Driven**: Rich entities with domain methods
- **JWT Auth**: Role + Permission-based authorization

---

## Configuration

**Database**: `Server=.;Database=EPMSDb;User Id=sa;Password=12345`

**JWT Settings**:
- Issuer: `EPMS-Backend`
- Audience: `EPMS-Frontend`
- Access Token: 60 min
- Refresh Token: 7 days
# EPMS - Agent Instructions

## Quick Start

```powershell
# Build solution
dotnet build

# Run API (EPMS.Api)
cd EPMS.Api; dotnet run

# Run Client (EPMS.Client)
cd EPMS.Client; dotnet run
```

- Swagger: `https://localhost:<port>/swagger` (dev only)
- Default credentials: `sa2026@gmail.com` / `Admin@123`

## Project Structure

```
EPMS_Solution/
├── EPMS.Api/           # Web API (Controllers, Extensions, Filters)
├── EPMS.Client/        # Blazor WebAssembly SPA
├── EPMS.Domain/        # Entities, Services, Repositories, Data
├── EPMS.Shared/        # DTOs, Validators, Constants
└── EPMS_Solution.slnx  # Solution file
```

## Architecture Patterns

- **Clean Architecture**: 4-layer separation (Api → Domain → Shared)
- **Unit of Work**: 6 modules (`App`, `Auth`, `Info`, `HR`, `Perf`, `Shared`)
- **Soft Delete**: Global EF query filter via `ISoftDeletable`
- **Audit Trail**: `AuditableEntity` base + `AuditInterceptor`
- **Scrutor DI**: Auto-registered by assembly scanning

## Database

- Connection: `Server=.;Database=EPMSDb;User Id=sa;Password=12345`
- Migrations: `EPMS.Domain/Data/Migrations`
- Auto-seed on startup

## JWT Settings

- Issuer: `EPMS-Backend`, Audience: `EPMS-Frontend`
- Access: 60 min, Refresh: 7 days

## Roles

| Id | Name |
|----|------|
| 1 | SystemAdmin |
| 2 | Admin |
| 3 | User |

## Validation Messages

All validation messages are centralized in `EPMS.Shared/Constants/ValidationMessages/`:
- `AuthValidationMessages.cs`
- `EmployeeInfoValidationMessages.cs`
- `HrValidationMessages.cs`
- `PerformanceValidationMessages.cs`
- `PermissionValidationMessages.cs`
- `SharedValidationMessages.cs`

Use constants instead of hardcoded strings in validators.
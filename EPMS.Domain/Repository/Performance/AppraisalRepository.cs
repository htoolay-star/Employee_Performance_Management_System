using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.Irepo.Performance;
using EPMS.Domain.Repository.Base;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.FormDTOs;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Performance
{
    public class AppraisalRepository : GenericRepository<Appraisal>, IAppraisalRepository
    {
        public AppraisalRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Appraisal>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(a => a.Employee)
                .Include(a => a.Cycle)
                .Include(a => a.ManagerReviewer)
                .ToListAsync(cancellationToken);
        }

        public async Task<Appraisal?> GetAppraisalWithDetailsAsync(long id)
        {
            return await _dbSet
                .AsNoTrackingWithIdentityResolution()
                .AsSplitQuery()
                .Include(a => a.Employee)
                    .ThenInclude(e => e.Employment)
                        .ThenInclude(emp => emp.Position)
                .Include(a => a.Employee)
                    .ThenInclude(e => e.Employment)
                        .ThenInclude(emp => emp.Department)
                .Include(a => a.Employee)
                    .ThenInclude(e => e.Employment)
                        .ThenInclude(emp => emp.Team)
                .Include(a => a.Employee)
                    .ThenInclude(e => e.Employment)
                        .ThenInclude(emp => emp.DirectManager)
                .Include(a => a.ManagerReviewer)
                .Include(a => a.Cycle)
                .Include(a => a.Details)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<AppraisalFillDto?> GetAppraisalFillDtoAsync(long id)
        {
            var dto = await _dbSet
                .Where(a => a.Id == id)
                .Select(a => new AppraisalFillDto
                {
                    Id = a.Id,
                    EmployeeId = a.EmployeeId ?? 0,
                    EmployeeName = a.Employee != null ? a.Employee.StaffName : null,
                    StaffNo = a.Employee != null ? a.Employee.StaffNo : string.Empty,
                    PositionName = a.Employee != null && a.Employee.Employment != null
                        ? a.Employee.Employment.Position.Name : null,
                    DepartmentName = a.Employee != null && a.Employee.Employment != null
                        ? a.Employee.Employment.Department.Name : null,
                    TeamName = a.Employee != null && a.Employee.Employment != null && a.Employee.Employment.Team != null
                        ? a.Employee.Employment.Team.Name : null,
                    ManagerName = a.Employee != null && a.Employee.Employment != null && a.Employee.Employment.DirectManager != null
                        ? a.Employee.Employment.DirectManager.StaffName : "Admin Team",
                    CycleId = a.CycleId,
                    CycleName = a.Cycle != null ? a.Cycle.Name : null,
                    ManagerReviewerId = a.ManagerReviewerId,
                    ManagerReviewerName = a.ManagerReviewer != null ? a.ManagerReviewer.StaffName : null,
                    Status = a.Status,
                    IsLocked = a.IsLocked,
                    KpiLocked = a.KpiLocked,
                    KpiStatus = a.KpiStatus ?? "DRAFT",
                    SelfStatus = a.SelfStatus ?? "DRAFT",
                    ManagerStatus = a.ManagerStatus ?? "DRAFT",
                    PeerStatus = a.PeerStatus ?? "DRAFT",
                    SubordinateStatus = a.SubordinateStatus ?? "DRAFT",
                    CommitteeStatus = a.CommitteeStatus ?? "DRAFT",
                    EntityType = a.EntityType,
                    EntityId = a.EntityId,
                    Details = a.Details.Select(d => new AppraisalDetailFillDto
                    {
                        KPIId = d.KPIId,
                        KPIName = d.KPIName,
                        CategoryName = d.CategoryName,
                        Weightage = d.Weightage,
                        TargetValue = d.TargetValue,
                        ScoringDirection = d.ScoringDirection,
                        ActualValue = d.ActualValue,
                        Score = d.Score,
                        WeightedScore = d.WeightedScore,
                        Remarks = d.Remarks,
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (dto == null) return null;

            if (dto.EntityType == AppraisalConstants.EntityTypes.Department && dto.EntityId.HasValue)
            {
                var department = await _context.Departments
                    .AsNoTracking()
                    .Include(d => d.DeptHead)
                    .FirstOrDefaultAsync(d => d.Id == dto.EntityId.Value);

                dto.EntityName = department?.Name;
                dto.EntityHeadName = department?.DeptHead?.StaffName;
            }
            else if (dto.EntityType == AppraisalConstants.EntityTypes.Team && dto.EntityId.HasValue)
            {
                var team = await _context.Teams
                    .AsNoTracking()
                    .Include(t => t.LeadTeam)
                    .FirstOrDefaultAsync(t => t.Id == dto.EntityId.Value);

                dto.EntityName = team?.Name;
                dto.EntityHeadName = team?.LeadTeam?.StaffName;
            }

            return dto;
        }

        public async Task<IEnumerable<Appraisal>> GetEmployeeAppraisalsAsync(long employeeId, int cycleId)
        {
            return await _dbSet
                .Where(a => a.EmployeeId == employeeId && a.CycleId == cycleId)
                .Include(a => a.Details)
                .ToListAsync();
        }

        public async Task<bool> ExistsByEmployeeAndCycleAsync(long employeeId, long cycleId)
        {
            return await _dbSet.AnyAsync(a =>
                a.EmployeeId == employeeId &&
                a.CycleId == cycleId);
        }

        public async Task<bool> ExistsByEntityAndCycleAsync(string entityType, long entityId, long cycleId)
        {
            return await _dbSet.AnyAsync(a =>
                a.EntityType == entityType &&
                a.EntityId == entityId &&
                a.CycleId == cycleId);
        }

        public async Task<IEnumerable<Appraisal>> GetByEntityAndCycleAsync(string entityType, long entityId, long cycleId)
        {
            return await _dbSet
                .Where(a => a.EntityType == entityType && a.EntityId == entityId && a.CycleId == cycleId)
                .Include(a => a.Details)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appraisal>> GetByManagerReviewerIdAsync(long managerReviewerId)
        {
            return await _dbSet
                .Where(a => a.ManagerReviewerId == managerReviewerId && !a.IsDeleted)
                .AsNoTracking()
                .Include(a => a.Employee)
                .Include(a => a.Cycle)
                .Include(a => a.ManagerReviewer)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appraisal>> GetAppraisalsByCycleAsync(long cycleId)
        {
            return await _dbSet
                .Where(a => a.CycleId == cycleId)
                .Include(a => a.Employee)
                .Include(a => a.Details)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appraisal>> GetByNoDirectManagerAsync()
        {
            return await _dbSet
                .Where(a => a.Employee != null
                    && a.Employee.Employment != null
                    && a.Employee.Employment.DirectManagerId == null
                    && !a.IsDeleted)
                .AsNoTracking()
                .Include(a => a.Employee)
                .Include(a => a.Cycle)
                .Include(a => a.ManagerReviewer)
                .ToListAsync();
        }
    }
}
